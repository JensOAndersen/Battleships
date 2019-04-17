using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipsGame.Utils
{
    public class Converters
    {
        /// <summary>
        /// Converts a letter to the corresponding integer
        /// </summary>
        /// <param name="letter">Letter to be converted</param>
        /// <returns>the corresponding integer a=0, b=1 etc</returns>
        public static int CharToInt(char letter)
        {
            if (letter >= 'a' && letter <= 'j')
            {
                return letter - 97;
            } else
            {
                throw new ArgumentException("Please enter a letter between 'a' and 'j' (both included)");
            }
        }

        public static char IntToChar(int num)
        {
            if (num >= 0 && num <= 9)
            {
                return (char)(num + 97);
            } else
            {
                throw new ArgumentException("This method only accepts ints between 0 and 9 (both included)");
            }
        }

        /// <summary>
        /// Converts a string to a coordinate set
        /// </summary>
        /// <param name="str">String in the format [a-j][0-9]</param>
        /// <returns>Converted x and y values</returns>
        public static (int x, int y) StringToCoordinate(string str)
        {
            str = str.Trim();
            var validationResult = Validators.IsValidCoordinate(str);
            if (validationResult.isValid)
            {
                int x = CharToInt(str[0]);
                int y = int.Parse(str[1].ToString());

                return (x, y);
            }
            else
            {
                throw new ArgumentException("String is an invalid coordinate");
            }
        }
    }
}
