using System;

namespace MultiplayerSnake.database.data
{
    public class FoodsData
    {
        public FoodsData() { }

        public FoodsData(int x, int y, int level, string uuid)
        {
            this.x = x;
            this.y = y;
            this.level = level;
            this.uuid = uuid;
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
        public int level { get; set; }

        /// <summary>
        /// Each food has its own identifier, this fixes synchronization problems
        /// </summary>
        public string uuid { get; set; }
    }
}
