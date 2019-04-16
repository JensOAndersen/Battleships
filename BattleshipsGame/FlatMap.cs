using System;
using System.Collections.Generic;
using System.Text;
using BattleshipsGame.Utils;

namespace BattleshipsGame
{

    /*
     * You are working on the validation methods, splitting them up sot he methods only return a single coherent value
     */
    public class FlatMap
    {
        private string[,] map;

        public const int mapXSize = 10;
        public const int mapYSize = 10;

        //this needs to be refactored so the map itself cant be accessed outside the flatmap class
        public string[,] Map
        {
            get
            {
                return map;
            }
        }

        public FlatMap()
        {

            map = new string[mapYSize, mapXSize];

            PopulateMap(map);
        }

        /// <summary>
        /// Sends a shot towards a coordinate
        /// </summary>
        /// <param name="str">The string coordinates in the format[a-j[0-9]</param>
        /// <returns>Whether its valid or not, the validation message</returns>
        public (bool isValid, string message) MarkCoordinate(string str, char icon)
        {
            str = str.Trim();

            int x = 0;
            int y = 0;

            try
            {
                var validationResult = Converters.StringToCoordinate(str);

                x = validationResult.x;
                y = validationResult.y;
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

            if (
                map[y, x] == "x" ||
                map[y, x] == "o" 
                )
            {
                return (false, "This coordinate has already been marked");
            }
            else
            {
                map[y, x] = icon.ToString();
                return (true, "Success!");
            }
        }




        #region Static Methods

        /// <summary>
        /// Populates the inputted player map
        /// </summary>
        /// <param name="input">The map to be populated</param>
        private static void PopulateMap(string[,] input)
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
        public static string[] CreateMap(FlatMap map)
        {
            List<string> output = new List<string>();

            string[,] workingMap = map.Map;

            output.Add("------------------------------------------");

            for (int y = 0; y < workingMap.GetLength(0); y++)
            {
                string line = "|";

                for (int x = 0; x < workingMap.GetLength(1); x++)
                {
                    line += $" {workingMap[y, x]} |";
                }
                output.Add(line);
                output.Add("------------------------------------------");
            }

            return output.ToArray();
        }


        #endregion Static Methods
    }
}
