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
            //test;
        }
    }
}
