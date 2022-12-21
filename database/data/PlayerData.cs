using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.database.data
{
    class PlayerData
    {
        public string color { get; set; }

        public string verifyName { get; set; }

        public ConcurrentBag<PlayerPositionData> pos { get; set; }
    }
}
