namespace MultiplayerSnake.database.data
{
    public class PlayerPositionData
    {
        public PlayerPositionData() { }

        public PlayerPositionData(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Player x position
        /// </summary>
        public int x { get; set; }

        /// <summary>
        /// Player y position
        /// </summary>
        public int y { get; set; }
    }
}
