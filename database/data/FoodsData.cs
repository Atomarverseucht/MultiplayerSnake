using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.database.data
{
    public class FoodsData
    {
        /// <summary>
        /// The level of the food
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// The x position of the food
        /// </summary>
        public int x { get; set; }

        /// <summary>
        /// The y position of the food
        /// </summary>
        public int y { get; set; }
    }
}
