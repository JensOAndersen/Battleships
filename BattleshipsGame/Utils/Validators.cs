using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipsGame.Utils
{
    public class Validators
    {
        /// <summary>
        /// Checks if a coordinate is valid
        /// </summary>
        /// <param name="str">string in the format [a-j][0-9]</param>
        /// <returns>the result of the validation</returns>
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

            char charXStart = input[0];
            int yValue = 0;
            int xValue = 0;

            if (!int.TryParse(input[1].ToString(), out yValue))
            {
                return (false, "The row value is invalid");
            }

            xValue = Converters.LetterToInt(charXStart);

            //test if x and y is within map, and if you have targeted the same location before
            int yMin = 0;
            int xMin = 0;
            int yMax = FlatMap.mapYSize;
            int xMax = FlatMap.mapXSize;


            if (xValue >= xMin &&
                xValue < xMax &&
                yValue >= yMin &&
                yValue < yMax)
            {
                return (true, "Success");
            }

            return (false, "You're aiming outside the map");
        }

    }
}
