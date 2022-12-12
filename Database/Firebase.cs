using Firebase.Database;
using MultiplayerSnake.Database;
using System;
using System.Threading.Tasks;

namespace MultiplayerSnake
{
    class Firebase
    {
        // the database client
        private FirebaseClient client;

        public Firebase()
        {
            this.client = new FirebaseClient(
                DatabaseConstants.FIREBASE_URL,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(DatabaseConstants.FIREBASE_AUTH)
                }
            );
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
    }
}
