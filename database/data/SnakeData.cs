using MultiplayerSnake.database.data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.database
{
    public class SnakeData
    {
        public List<FoodsData> foods;
        public ConcurrentDictionary<string, int> highscores;
        public ConcurrentDictionary<string, PlayerData> players;
        public VariablesData variables;
    }
}
