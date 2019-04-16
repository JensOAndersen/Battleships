using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipsGame
{
    public class Player
    {
        public string Name { get; set; }
        public string[,] ShipMap { get; set; } = new string[10, 10];
        public string[,] HitMap { get; set; } = new string[10, 10];
        public Player()
        {

            PopulateMap(ShipMap);
            PopulateMap(HitMap);
        }

        /*
         * This should be refactored into a MapLogic class, and an ILogic Interface, to support different type of map logic.
         * 
         * The current map is reliant on a 2d array of empty strings, it can be refactored into a dictionary instead, 
         * it would make it easier to look through.
         * 
         * The current 2D Array solution is because its easier for me to visualize when making the game.
         */
        /// <summary>
        /// Populates the inputted player map
        /// </summary>
        /// <param name="input">The map to be populated</param>
        private void PopulateMap(string[,] input)
        {
            string[,] map = input;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    map[y, x] = " ";
                }
            }
        }
    }
}
