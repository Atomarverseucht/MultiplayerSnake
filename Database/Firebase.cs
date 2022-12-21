using Firebase.Database;
using MultiplayerSnake.database.data;
using MultiplayerSnake.Database;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiplayerSnake
{
    class Firebase
    {
        // the database client
        private FirebaseClient client;

        public ConcurrentDictionary<int, FoodsData> foods = new ConcurrentDictionary<int, FoodsData>();

        public string name = "";

        public string snake_color = "";

        public ConcurrentDictionary<string, PlayerData> otherSnakes = new ConcurrentDictionary<string, PlayerData>();

        public ConcurrentDictionary<string, PlayerData> allSnakes = new ConcurrentDictionary<string, PlayerData>();

        public int forcedFoodLevel = -1;


        public Firebase()
        {
            this.client = new FirebaseClient(Constants.FIREBASE_URL, Constants.FIREBASE_CONFIG);


        }

        /// <summary>
        /// Queries the database synchronous with the main thread for a specific key under the snake parameter.
        /// </summary>
        /// <typeparam name="T">The type of the data stored</typeparam>
        /// <param name="key">The key to query</param>
        /// <returns></returns>
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
        /// Querys the database for the version and compares it with the clients version.
        /// </summary>
        /// <returns>true if the version is up to date</returns>
        public bool checkVersion()
        {
            return this.checkVersion(this.queryOnce<int>(Constants.FIREBASE_KEY_VERSION));
        }

        /// <summary>
        /// Compares the given version with the clients version.
        /// </summary>
        /// <param name="dbVersion">the current version of the database</param>
        /// <returns></returns>
        private bool checkVersion(int dbVersion)
        {
            Console.WriteLine(dbVersion);
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

        // set the listeners for firebase
        public void registerFireBaseListeners()
        {
            ManualResetEvent oSignalEvent = new ManualResetEvent(false);

            // value of foods changed, update it
            this.client.Child("snake/foods").AsObservable<FoodsData>((sender, e) => Console.WriteLine(e.Exception), "").Subscribe(snapshot =>
            {
                this.foods.AddOrUpdate(int.Parse(snapshot.Key), snapshot.Object, (key, oldValue) => snapshot.Object);

                // if first run, signal main thread to continue
                oSignalEvent.Set();
            });
            oSignalEvent.WaitOne();

            // the food spawn type is forced by database
            this.client.Child("snake/variables").AsObservable<VariablesData>((sender, e) => Console.WriteLine(e.Exception), "forcedFoodLevel").Subscribe(snapshot =>
            {
                // set new forced food level
                this.forcedFoodLevel = snapshot.Object.forcedFoodLevel;

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
                string key = snapshot.Key;
                PlayerData playerData = snapshot.Object;

                if (playerData.pos != null && playerData.pos.Any())
                    Console.WriteLine(playerData.pos.ElementAt(0).x);

                if (string.IsNullOrWhiteSpace(key) || playerData == null)
                {
                    oSignalEvent.Set();
                    return;
                }

                // update all snakes (used to count online (registered) players)
                this.allSnakes[key] = playerData;

                // check if the update data is for us
                if (key == this.name)
                {
                    // then we can set our own color
                    this.snake_color = this.allSnakes[this.name].color;
                }

                // we need to have a seperate dict with only other players
                this.otherSnakes = new ConcurrentDictionary<string, PlayerData>(this.allSnakes);
                // we ignore ourself
                this.otherSnakes.TryRemove(this.name, out var ignored);

                // if first run, signal main thread to continue
                oSignalEvent.Set();
            });
            oSignalEvent.WaitOne();
        }
    }
}
