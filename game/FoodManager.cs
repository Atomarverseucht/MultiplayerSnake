using MultiplayerSnake.database.data;
using MultiplayerSnake.Database;
using MultiplayerSnake.utils;
using System.Collections.Concurrent;

namespace MultiplayerSnake.game
{
    public class FoodManager
    {
        // access to main form
        private MainForm mainForm;

        // access to database
        private Firebase firebase;

        // access to the manger class for players
        private PlayerManager playerManager;

        // food positions
        public ConcurrentDictionary<int, FoodsData> foods = new ConcurrentDictionary<int, FoodsData>();

        // if not negative, only this type of food will spawn
        public int forcedFoodLevel = -1;

        public FoodManager(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        /// <summary>
        /// This must be called, when all class variables in main form have been set.
        /// </summary>
        public void init()
        {
            this.firebase = this.mainForm.firebase;
            this.playerManager = this.mainForm.playerManager;
        }

        public void addFood(int x, int y, int level)
        {
            this.foods.AddOrUpdate(this.foods.Count, new FoodsData(x, y, level), (oldKey, oldValue) => new FoodsData(x, y, level));

            this.firebase.updateFoods();
        }

        public int randomFoodLevel()
        {
            // if there is a food level forced by database, use it
            if (this.forcedFoodLevel >= 0)
            {
                return this.forcedFoodLevel;
            }

            int rnd = Utils.RANDOM.Next(301);

            if (rnd >= 0 && rnd < 100)
            {
                // less and medium are most common
                return Constants.FOOD_LEVEL_LESS;
            }
            else if (rnd >= 100 && rnd < 200)
            {
                // less and medium are most common
                return Constants.FOOD_LEVEL_MEDIUM;
            }
            else if (rnd >= 200 && rnd < 250)
            {
                // much and random are less common
                return Constants.FOOD_LEVEL_MUCH;
            }
            else
            {
                // much and random are less common
                return Constants.FOOD_LEVEL_RANDOM;
            }
        }

        /// <summary>
        /// Check if there are any foods. If not, add some
        /// </summary>
        public void checkFoodsAvailable()
        {
            if (this.playerManager.getActivePlayers() <= 1 && this.foods.Count == 0)
            {
                addFood(Utils.randomCoordinateX(), Utils.randomCoordinateY(), randomFoodLevel());
            }
        }
    }
}
