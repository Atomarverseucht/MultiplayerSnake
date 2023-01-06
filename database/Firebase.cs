using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using MultiplayerSnake.database;
using MultiplayerSnake.database.data;
using MultiplayerSnake.Database;
using MultiplayerSnake.game;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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

        // the task currently running to update the database
        private Task updateTask = Task.CompletedTask;

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
        /// <param name="async">if we should wait for the task to complete</param>
        /// <returns>if the data was set successfully</returns>
        public bool put<T>(string key, T value, bool async=false)
        {
            try
            {
                if (async)
                {
                    if (this.updateTask == null || this.updateTask.IsCompleted)
                    {
                        // when there is no currently running update task, create one
                        lock (this.updateTask) lock (value)
                            {
                                this.updateTask = this.client.Child("snake/" + key).PutAsync(value);
                            }
                    }
                    else
                    {
                        lock (this.updateTask)
                        {
                            // else, wait for the current one to finish
                            this.updateTask.ContinueWith(t =>
                            {
                                lock (this.updateTask) lock (value)
                                    {
                                        this.updateTask = this.client.Child("snake/" + key).PutAsync(value);
                                    }
                            });
                        }
                    }
                }
                else
                {
                    Task.WaitAll(this.updateTask);
                    var task = this.client.Child("snake/" + key).PutAsync<T>(value);
                    Task.WaitAll(task);
                }

                
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
            lock (this.foodManager.foods)
            {
                this.put(Constants.FIREBASE_FOODS_KEY, this.foodManager.foods, true);
            }
        }

        /// <summary>
        /// save our playerdata to database
        /// </summary>
        /// <param name="snakeData">the position data to save</param>
        public void setPlayerData(List<PlayerPositionData> snakeData)
        {
            if (this.playerManager.isInvisibleForOthers)
            {
                snakeData = new List<PlayerPositionData>();
            }
            this.put(Constants.FIREBASE_PLAYER_POS_KEY.Replace("%name%", this.playerManager.name), snakeData, true);
        }

        // set the listeners for firebase
        public void registerFireBaseListeners()
        {
            this.client.Child("").AsObservable<SnakeData>((sender, e) => Console.Error.WriteLine(e.Exception), "").Subscribe(snapshot =>
            {

                SnakeData snakeData = snapshot.Object;
                if (snakeData == null)
                {
                    return;
                }

                // --------- foods ---------

                this.foodManager.foods = snakeData.foods == null
                    ? new ConcurrentDictionary<int, FoodsData>()
                    : new ConcurrentDictionary<int, FoodsData>(snakeData.foods.Select((s, i) => new { s, i }).ToDictionary(x => x.i, x => x.s));

                // --------- variables ---------

                // set new forced food level
                this.foodManager.forcedFoodLevel = snakeData.variables.forcedFoodLevel;

                // check for version changes, if version changed, close app
                if (!this.checkVersion(snakeData.variables.version))
                {
                    Application.Exit();
                    return;
                }

                // --------- player ---------

                if (snakeData.players != null)
                {
                    this.playerManager.allSnakes = snakeData.players;
                    foreach (KeyValuePair<string, PlayerData> player in snakeData.players)
                    {
                        string key = player.Key;
                        PlayerData playerData = player.Value;

                        // check if the update data is for us and the color is not null or empty
                        if (key == this.playerManager.name && !string.IsNullOrWhiteSpace(playerData.color))
                        {
                            // then we can set our own color
                            this.playerManager.color = playerData.color;
                        }

                        // we need to have a seperate dict with only other players
                        this.playerManager.otherSnakes = new ConcurrentDictionary<string, PlayerData>(this.playerManager.allSnakes);
                        this.playerManager.otherSnakes.TryRemove(this.playerManager.name, out var ignored3);
                    }
                }
                else
                {
                    this.playerManager.allSnakes.Clear();
                    this.playerManager.otherSnakes.Clear();
                }
            });
        }
    }
}
