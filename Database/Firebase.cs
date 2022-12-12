using Firebase.Database;
using MultiplayerSnake.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiplayerSnake
{
    class Firebase
    {
        // the database client
        private FirebaseClient client;

        public bool firstInitOtherSnakes = false;

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
        public void setFireBaseListeners()
        {
            // value of foods changed, update it
            this.client.Child("snake/foods").AsObservable<Dictionary<string, string>[]>().Subscribe((Dictionary<string, string>[] data) =>
            {
                this.foods = data == null ? [] : data;
            }
            );
            firebase.database().ref ("snake/foods").on("value", (snapshot) => {
                data = snapshot.val();
                foods = data == null?[] : data;
            });

            // the food spawn type is forced by database
            firebase.database().ref ("snake/forcedFoodLevel").on("value", (snapshot) => {
                data = snapshot.val();
                forcedFoodLevel = data == null ? -1 : data;
            });

            // listen for other snake(s) changes
            firebase.database().ref ("snake/players").on("value", (snapshot) => {
                data = snapshot.val();
                if (data == null)
                {
                    this.firstInitOtherSnakes = true;
                    return;
                }

                var newArray = [];
      // we ignore ourself, so we have to put it in a new array, without ourself
      for (var playerName in data)
                {
                    if (playerName == name)
                    {
                        // update our own color
                        my_snake_col = data[playerName]["color"];
                        continue;
                    }
                    newArray[playerName] = data[playerName];
                }

                otherSnakes = newArray;
                // used to count online (registered) players
                allSnakes = data;
                this.firstInitOtherSnakes = true;
            });
        }
    }
}
