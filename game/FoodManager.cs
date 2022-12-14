using MultiplayerSnake.database.data;
using MultiplayerSnake.Database;
using MultiplayerSnake.utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;

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

        // lightness of all foods. the foods are slowly sweeping up
        // and down in brightness to show, that they are foods.
        private int foodLightness = 0;

        // food positions
        public ConcurrentDictionary<int, FoodData> foods { get; set; } = new ConcurrentDictionary<int, FoodData>();

        // this list contains newly removed foods, so that the next database update doesn't mess with our food data
        // the foods each have a uuid. if a food gets removed, it needs to be put in the removed foods
        // because if we receive an update from the database, it could still be a tiny bit outdated data
        public List<string> removedFoods { get; set; } = new List<string>();

        // if not negative, only this type of food will spawn
        public int forcedFoodLevel { get; set; } = -1;

        public FoodManager(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        /// <summary>
        /// This must be called when all class variables in main form have been set.
        /// </summary>
        public void init()
        {
            this.firebase = this.mainForm.firebase;
            this.playerManager = this.mainForm.playerManager;
        }

        /// <summary>
        /// save a new food position
        /// </summary>
        /// <param name="x">x pos of the food</param>
        /// <param name="y">y pos of the food</param>
        /// <param name="level">the level of the food</param>
        public void addFood(int x, int y, int level)
        {
            this.addFood(new FoodData(x, y, level, Guid.NewGuid().ToString()));
        }

        /// <summary>
        /// save a new food position
        /// </summary>
        /// <param name="foodData">the food data to save</param>
        public void addFood(FoodData foodData)
        {
            lock (this.foods)
            {
                this.foods.TryAdd(this.foods.Count, foodData);
            }
        }

        /// <summary>
        /// remove a food
        /// </summary>
        /// <param name="x">the x position of the food to remove</param>
        /// <param name="y">the y position of the food to remove</param>
        /// <returns>the level of the food removed</returns>
        public int removeFood(int x, int y)
        {
            // the new food data
            ConcurrentDictionary<int, FoodData> newFoods = new ConcurrentDictionary<int, FoodData>();
            // the removed food
            FoodData oldFood = new FoodData(0, 0, Constants.FOOD_LEVEL_RANDOM, Guid.NewGuid().ToString());

            lock (this.foods)
            {
                foreach (FoodData food in this.foods.Values)
                {
                    // getting the data of the other food
                    int foodX = food.x;
                    int foodY = food.y;

                    // if the data isn't equal to our data, we will add it to the new list
                    if (x != foodX || y != foodY)
                    {
                        newFoods.TryAdd(newFoods.Count, food);
                    }
                    else
                    {
                        oldFood = food;
                        // we don't need to care about database sync issues, when we are offline
                        if (!this.firebase.isOffline())
                        {
                            this.removedFoods.Add(food.uuid);
                        }
                    }
                }
            }

            // update the food array
            lock (this.foods)
            {
                this.foods = newFoods;
            }
            this.firebase.updateFoods();

            return oldFood.level;
        }

        /// <summary>
        /// generate a new random food location
        /// </summary>
        public FoodData genFood()
        {
            // Generate a random number the food x-coordinate
            var foodX = Utils.randomCoordinateX();
            // Generate a random number for the food y-coordinate
            var foodY = Utils.randomCoordinateY();

            // if the new food location is where a snake currently is, generate a new food location
            foreach (PlayerData playerData in this.playerManager.allSnakes.Values)
            {
                if (playerData.pos == null)
                    continue;

                foreach (PlayerPositionData part in playerData.pos)
                {
                    if (part.x == foodX && part.y == foodY)
                    {
                        this.genFood();
                    }
                }
            }

            return new FoodData(foodX, foodY, this.randomFoodLevel(), Guid.NewGuid().ToString());
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
            lock (this.foods)
            {
                if (this.foods.Count == 0)
                {
                    lock (this.foods)
                    {
                        this.addFood(Utils.randomCoordinateX(), Utils.randomCoordinateY(), randomFoodLevel());
                        this.firebase.updateFoods();
                    }
                }
            }
            
        }

        /// <summary>
        /// draws all foods in the food dictionary
        /// </summary>
        /// <param name="g"></param>
        public void drawFoods(Graphics g)
        {
            lock (this.foods)
            {
                foreach (FoodData food in this.foods.Values)
                {
                    lock (this.foods)
                    {
                        Rectangle rect = new Rectangle(food.x, food.y, 10, 10);
                        g.FillRectangle(new SolidBrush(this.currentFoodLightness(Utils.getFoodColorByLevel(food.level))), rect);
                        g.DrawRectangle(Pens.Black, rect);
                    }
                }
            }

            // increase lightness
            this.foodLightness++;
        }

        /// <summary>
        /// get the current food color with the blink effect
        /// </summary>
        /// <param name="hue">the color of the food</param>
        /// <returns>the color with applied lightness</returns>
        public Color currentFoodLightness(double hue)
        {
            if (this.foodLightness >= 50)
            {
                this.foodLightness = -50;
            }

            return Utils.ColorFromHSV(hue, 1, ((this.foodLightness < 0 ? (this.foodLightness * -1) : this.foodLightness) + 50) / 100.0);
        }

        /// <summary>
        /// check if a player has eaten a food
        /// </summary>
        /// <returns></returns>
        public bool hasEatenFood()
        {
            lock (this.foods)
            {
                foreach (FoodData food in this.foods.Values)
                {
                    lock (this.foods)
                    {
                        if (this.playerManager.snake[0].x == food.x && this.playerManager.snake[0].y == food.y)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// handle the collection of food
        /// </summary>
        /// <returns>the level of the removed food</returns>
        public int handleFoodCollect()
        {
            lock (this.foods)
            {
                // removing old food
                int removedOldFoodLevel = this.removeFood(this.playerManager.snake[0].x, this.playerManager.snake[0].y);

                // creating new food
                int foodCount = Math.Max(1, (int)Math.Round(this.playerManager.getActivePlayers() / 2.0));
                foodCount = foodCount - this.foods.Count;

                for (var i = 0; i < foodCount; i++)
                {
                    FoodData randomFood = null;
                    bool noFoodPosFound = true;

                    // search for a food pos, which isn't in use
                    while (noFoodPosFound)
                    {
                        randomFood = this.genFood();

                        // there are no positions to check
                        lock (this.foods)
                        {
                            if (this.foods.Count == 0)
                                noFoodPosFound = false;
                        }

                        lock (this.foods)
                        {
                            // check all other food positions to avoid overlap
                            foreach (FoodData food in this.foods.Values)
                            {
                                lock (this.foods)
                                {
                                    if (food.x != randomFood.x || food.y != randomFood.y)
                                    {
                                        noFoodPosFound = false;
                                    }
                                }
                            }
                        }
                    }

                    // and add it, when found
                    this.addFood(randomFood);
                }

                this.firebase.updateFoods();

                return removedOldFoodLevel;
            }
        }

        /// <summary>
        /// drop random foods when the player died
        /// </summary>
        public void dropRandomFood()
        {
            // don't drop, if player has score 0
            if (this.playerManager.snake.Count <= 5)
                return;

            foreach (PlayerPositionData part in this.playerManager.snake)
            {
                // ignore this pos, if it is in the wall...
                if ((part.x < 0 || part.x > (Constants.SNAKEBOARD_MAX_X - 10))
                    || (part.y < 0 || part.y > (Constants.SNAKEBOARD_MAX_Y - 10)))
                    continue;

                // drop random food at random positions in snake, ca. 16,67% probability to drop for each
                if (Utils.RANDOM.Next(6) == 0)
                    addFood(part.x, part.y, randomFoodLevel());
            }

            this.firebase.updateFoods();
        }
    }
}
