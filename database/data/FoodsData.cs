using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.database.data
{
    public class FoodsData
    {
        public FoodsData() { }

        public FoodsData(int x, int y, int level)
        {
            this.x = x;
            this.y = y;
            this.level = level;
        }


        /// <summary>
        /// The x position of the food
        /// </summary>
        public int x { get; set; }

        /// <summary>
        /// The y position of the food
        /// </summary>
        public int y { get; set; }
    
        /// <summary>
        /// The level of the food
        /// </summary>
        public int level { get; set; }}
}
