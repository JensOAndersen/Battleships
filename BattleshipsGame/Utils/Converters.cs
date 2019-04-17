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
        public static int LetterToInt(char letter)
        {
            if (letter >= 'a' && letter <= 'j')
            {
                return letter - 97;
            } else
            {
                throw new ArgumentException("Please enter a column coordinate between 'a' and 'j'");
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
                int x = LetterToInt(str[0]);
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
