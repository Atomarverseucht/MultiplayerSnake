using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using MultiplayerSnake.database.data;
using MultiplayerSnake.Database;
using MultiplayerSnake.game;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiplayerSnake
{
    public class Firebase
    {
        // access to main form
        private MainForm mainForm;

        // access to the manger class for foods
        private FoodManager foodManager;

        // access to the manger class for players
        private PlayerManager playerManager;

        // the database client
        private FirebaseClient client;

        public Firebase(MainForm mainForm)
        {
            this.client = new FirebaseClient(Constants.FIREBASE_URL, Constants.FIREBASE_CONFIG);

            this.mainForm = mainForm;

            this.foodManager = this.mainForm.foodManager;

            this.playerManager = this.mainForm.playerManager;
        }

        /// <summary>
        /// Queries the database synchronous with the main thread for a specific key under the snake parameter.
        /// </summary>
        /// <typeparam name="T">The type of the data stored</typeparam>
        /// <param name="key">The key to query</param>
        /// <returns>the requested data or null if it doesn't exist or an error occured</returns>
        public T queryOnce<T>(string key)
        {
            try
            {
                var task = this.client.Child("snake/" + key).OnceSingleAsync<T>();
                Task.WaitAll(task);
                return task.Result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return default;
            }
        }

        /// <summary>
        /// Adds or overrides data synchronous with the main thread at a given location in the database.
        /// </summary>
        /// <typeparam name="T">The type of the data to update/set</typeparam>
        /// <param name="key">the path to the data</param>
        /// <param name="value">the data to update/set</param>
        /// <returns>if the data was set successfully</returns>
        public bool put<T>(string key, T value)
        {
            try
            {
                var task = this.client.Child("snake/" + key).PutAsync<T>(value);
                Task.WaitAll(task);
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Delete data synchronous with the main thread from the database.
        /// </summary>
        /// <param name="key">the key to delete the data from</param>
        /// <returns>if the data was deleted successfully</returns>
        public bool delete(string key)
        {
            try
            {
                var task = this.client.Child("snake/" + key).DeleteAsync();
                Task.WaitAll(task);
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Querys the database for the version and compares it with the clients version.
        /// </summary>
        /// <returns>true if the version is up to date</returns>
        public bool checkVersion()
        {
            return this.checkVersion(this.queryOnce<int>(Constants.FIREBASE_VERSION_KEY));
        }

        /// <summary>
        /// Compares the given version with the clients version.
        /// </summary>
        /// <param name="dbVersion">the current version of the database</param>
        /// <returns></returns>
        private bool checkVersion(int dbVersion)
        {
            if (dbVersion > Constants.CLIENT_VERSION)
            {
                MessageBox.Show("Your client is outdated. Please update your client to the newest version.", "Error");
                return false;
            }
            else if (dbVersion < Constants.CLIENT_VERSION)
            {
                MessageBox.Show("The database is outdated. Please wait for the database to update.", "Error");
                return false;
            }

            return true;
        }

        /// <summary>
        /// save the foods data to database
        /// </summary>
        public void updateFoods()
        {
            this.put(Constants.FIREBASE_FOODS_KEY, this.foodManager.foods);
        }

        // set the listeners for firebase
        public void registerFireBaseListeners()
        {
            ManualResetEvent oSignalEvent = new ManualResetEvent(false);

            // value of foods changed, update it
            this.client.Child("snake/foods").AsObservable<FoodsData>((sender, e) => Console.WriteLine(e.Exception), "").Subscribe(snapshot =>
            {
                if (string.IsNullOrWhiteSpace(snapshot.Key))
                {
                    // if first run, signal main thread to continue
                    oSignalEvent.Set();
                    return;
                }
                int key = int.Parse(snapshot.Key);

                // check if the food got deleted
                if (snapshot.EventType == FirebaseEventType.Delete)
                {
                    // remove the food from the list
                    this.foodManager.foods.TryRemove(key, out var ignored);
                }
                else
                {
                    // add the food to the list
                    this.foodManager.foods.AddOrUpdate(key, snapshot.Object, (oldKey, oldValue) => snapshot.Object);
                }

                Console.WriteLine(JsonConvert.SerializeObject(this.foodManager.foods));

                // if first run, signal main thread to continue
                oSignalEvent.Set();
            });
            oSignalEvent.WaitOne();

            // the food spawn type is forced by database
            this.client.Child("snake/variables").AsObservable<VariablesData>((sender, e) => Console.WriteLine(e.Exception), "forcedFoodLevel").Subscribe(snapshot =>
            {
                // set new forced food level
                this.foodManager.forcedFoodLevel = snapshot.Object.forcedFoodLevel;

                // check for version changes, if version changed, close app
                if (!this.checkVersion(snapshot.Object.version))
                {
                    Application.Exit();
                }

                // if first run, signal main thread to continue
                oSignalEvent.Set();
            });
            oSignalEvent.WaitOne();

            // listen for other snake(s) changes
            this.client.Child("snake/players").AsObservable<PlayerData>().Subscribe(snapshot =>
            {
                if (string.IsNullOrWhiteSpace(snapshot.Key))
                {
                    // if first run, signal main thread to continue
                    oSignalEvent.Set();
                    return;
                }

                string key = snapshot.Key;
                PlayerData playerData = snapshot.Object;

                // check if we even have player data
                if (snapshot.EventType == FirebaseEventType.Delete || playerData == null)
                {
                    // remove the player
                    this.playerManager.allSnakes.TryRemove(key, out var ignored1);
                    this.playerManager.otherSnakes.TryRemove(key, out var ignored2);

                    oSignalEvent.Set();
                    return;
                }

                // TODO debug remove
                if (playerData.pos != null && playerData.pos.Any())
                    Console.WriteLine(playerData.pos.ElementAt(0).x);

                // update all snakes (used to count online (registered) players)
                this.playerManager.allSnakes[key] = playerData;

                // check if the update data is for us
                if (key == this.playerManager.name)
                {
                    // then we can set our own color
                    this.playerManager.color = this.playerManager.allSnakes[this.playerManager.name].color;
                }

                // we need to have a seperate dict with only other players
                this.playerManager.otherSnakes = new ConcurrentDictionary<string, PlayerData>(this.playerManager.allSnakes);
                // we ignore ourself
                this.playerManager.otherSnakes.TryRemove(this.playerManager.name, out var ignored3);

                // if first run, signal main thread to continue
                oSignalEvent.Set();
            });
            oSignalEvent.WaitOne();
        }
    }
}
