using Firebase.Database;
using Firebase.Database.Query;
using MultiplayerSnake.database.data;
using MultiplayerSnake.Database;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiplayerSnake
{
    class Firebase
    {
        // the database client
        private FirebaseClient client;

        public List<FoodData> foods = new List<FoodData>();

        public string name = "";

        public string snake_color = "";

        public Dictionary<string, PlayerData> otherSnakes = new Dictionary<string, PlayerData>();

        public Dictionary<string, PlayerData> allSnakes = new Dictionary<string, PlayerData>();

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
        /// Querys the database for the version and compares it against the clients version.
        /// </summary>
        /// <returns>true if the version is up to date</returns>
        public bool checkVersion()
        {
            int databaseVersion = this.queryOnce<int>(Constants.FIREBASE_KEY_VERSION);
            Console.WriteLine(databaseVersion);
            if (databaseVersion > Constants.CLIENT_VERSION)
            {
                MessageBox.Show("Your client is outdated. Please update your client to the newest version.", "Error");
                return false;
            }
            else if (databaseVersion < Constants.CLIENT_VERSION)
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
            this.client.Child("snake/foods").AsObservable<List<FoodData>>().Subscribe(snapshot =>
            {
                this.foods = snapshot.Object == null
                ? new List<FoodData>()
                : snapshot.Object;
                oSignalEvent.Set();
            });
            oSignalEvent.WaitOne();

            // the food spawn type is forced by database
            this.client.Child("snake/forcedFoodLevel").AsObservable<int>().Subscribe(snapshot =>
            {
                this.forcedFoodLevel = snapshot.Object;
                oSignalEvent.Set();
            });
            oSignalEvent.WaitOne();

            // listen for other snake(s) changes
            TaskCompletionSource<bool> tcs = null;
            this.client.Child("snake/players").AsObservable<Dictionary<string, PlayerData>>().Subscribe(snapshot =>
            {
                Dictionary<string, PlayerData> data = snapshot.Object;
                
                
                if (data == null)
                {
                    oSignalEvent.Set();
                    return;
                }

                // set our own color
                snake_color = data[this.name].color;

                // update all snakes (used to count online (registered) players)
                this.allSnakes = data;

                // we need to have a seperate dict with only other players
                this.otherSnakes = new Dictionary<string, PlayerData>(data);
                // we ignore ourself
                this.otherSnakes.Remove(this.name);

                oSignalEvent.Set();
            });
            oSignalEvent.WaitOne();
        }
    }
}
