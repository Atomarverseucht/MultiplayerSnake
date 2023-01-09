using MultiplayerSnake.database.data;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MultiplayerSnake.database
{
    public class SnakeData
    {
        public List<FoodData> foods { get; set; }
        public ConcurrentDictionary<string, int> highscores { get; set; }
        public ConcurrentDictionary<string, PlayerData> players { get; set; }
        public VariablesData variables { get; set; }
    }
}
