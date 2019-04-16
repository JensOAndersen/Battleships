using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipsGame
{

    /*
     * You are working on the validation methods, splitting them up sot he methods only return a single coherent value
     */ 
    public class FlatMap
    {
        private string[,] map;

        private const int mapXSize = 10;
        private const int mapYSize = 10;

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

        #region Static Methods
        /// <summary>
        /// Transforms and validates a coordinate string
        /// </summary>
        /// <param name="str">the string to be transformed into coordinates</param>
        /// <returns>Whether its valid or not, the validation message, the x and y positions</returns>
        public static(bool isValid, string message, int x, int y) StringToCoordinate(string str)
        {
            if (
                    player.HitMap[yValue, xValue] == "x" ||
                    player.HitMap[yValue, xValue] == "o")
            {
                return (false, "This coordinate has already been hit once", 0, 0);
            }

        }

        public static (bool isValid, string message) IsValidCoordinate(string str)
        {
            string input = str.Trim();

            if (string.IsNullOrEmpty(input))
            {
                return (false, "You are not allowed to enter an empty position");
            }

            if (input.Length != 2)
            {
                return (false, "The command is invalid");
            }

            string stringXStart = input[0].ToString();
            int yValue = 0;
            int xValue = 0;

            if (!int.TryParse(input[1].ToString(), out yValue))
            {
                return (false, "The row value is invalid");
            }

            xValue = LetterToInt(stringXStart);

            //test if x and y is within map, and if you have targeted the same location before
            int yMin = 0;
            int xMin = 0;
            int yMax = mapYSize;
            int xMax = mapXSize;


            if (xValue >= xMin &&
                xValue < xMax &&
                yValue >= yMin &&
                yValue < yMax)
            {
                return (true, "Success");
            }

            return (false, "You're aiming outside the map");
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

        public static int LetterToInt(string letter)
        {
            Dictionary<string, int> letterToInt = new Dictionary<string, int>
            {
                {"a",0 },
                {"b",1 },
                {"c",2 },
                {"d",3 },
                {"e",4 },
                {"f",5 },
                {"g",6 },
                {"h",7 },
                {"i",8 },
                {"j",9 },
            };

            if (letterToInt.ContainsKey(letter))
            {
                return letterToInt[letter];
            } else
            {
                throw new IndexOutOfRangeException("The letter given does not respond to a coordinate");
            }
        }
        #endregion Static Methods
    }
}
