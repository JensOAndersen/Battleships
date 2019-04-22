using System.Collections;
using System.Collections.Generic;

namespace BattleshipsGame
{
    public abstract class MapAbstract
    {
        protected Dictionary<(int x, int y),string> map;

        public Dictionary<(int x, int y), string> Map
        {
            get { return map; }
        }

        public const int mapXSize = 10;
        public const int mapYSize = 10;

        /// <summary>
        /// Sends a shot towards a coordinate
        /// </summary>
        /// <param name="str">The string coordinates in the format[a-j[0-9]</param>
        /// <returns>Whether its valid or not, the validation message</returns>
        public abstract (bool isValid, string message) ShootAtCoordinate((int x, int y) coordinate);

        /// <summary>
        /// Places a ship on the map
        /// </summary>
        /// <param name="str">the coordinate positions of the ship, as well as directions</param>
        /// <param name="ship">The ship to be placed</param>
        /// <returns>whether it succeeded or not, and a message</returns>
        public abstract (bool isValid, string message) PlaceShip(string str, Ship ship);



        #region Static Methods

        /// <summary>
        /// Populates the inputted player map
        /// </summary>
        /// <param name="input">The map to be populated</param>
        protected static void PopulateMap(string[,] input) //unused?
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

        /// <summary>
        /// Creates a visual representation of map as an array of lines
        /// </summary>
        /// <returns>an array of lines in a multiline map</returns>
        public static string[] CreateMap(MapAbstract map)
        {
            List<string> output = new List<string>();

            var workingMap = map.Map;

            output.Add("  | a | b | c | d | e | f | g | h | i | j |");

            output.Add("  -----------------------------------------");

            for (int y = 0; y < mapYSize; y++)
            {
                string line = y + " |";

                for (int x = 0; x < mapXSize; x++)
                {

                    (int x, int y) coordinate = (x, y);

                    string mapValue = " ";

                    if (workingMap.ContainsKey(coordinate))
                    {
                        mapValue = workingMap[coordinate];
                    }

                    line += $" {mapValue} |";
                }
                output.Add(line);
                output.Add("  -----------------------------------------");
            }

            return output.ToArray();
        }

        #endregion Static Methods
    }
}