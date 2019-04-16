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
            }
            else
            {
                throw new IndexOutOfRangeException("The letter given does not respond to a coordinate");
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
                int x = LetterToInt(str.Substring(0, 1));
                int y = int.Parse(str.Substring(1, 1));

                return (x, y);
            }
            else
            {
                throw new ArgumentException("String is an invalid coordinate");
            }
        }
    }
}
