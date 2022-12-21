using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.database.data
{
    public class VariablesData
    {
        /// <summary>
        /// The food level which is forced by the database
        /// </summary>
        public int forcedFoodLevel { get; set; }

        /// <summary>
        /// The database version
        /// </summary>
        public int version { get; set; }
    }
}
