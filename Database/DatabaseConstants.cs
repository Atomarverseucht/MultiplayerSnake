using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Database
{
    public static class DatabaseConstants
    {
        /// <summary>
        /// database url
        /// </summary>
        public const string FIREBASE_URL = "https://psyched-canto-311609-default-rtdb.europe-west1.firebasedatabase.app";

        /// <summary>
        /// database app secret
        /// </summary>
        public const string FIREBASE_AUTH = "AIzaSyBbRpK_BcltEmRQzLAUCFykMHEq5PQWWz4";

        /// <summary>
        /// the version of the client
        /// </summary>
        public const int CLIENT_VERSION = 0;

        /// <summary>
        /// the database key for the version
        /// </summary>
        public const string KEY_VERSION = "version";
    }
}
