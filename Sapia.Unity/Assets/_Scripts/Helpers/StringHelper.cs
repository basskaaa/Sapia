using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assets._Scripts.Helpers
{
    public static class StringHelper
    {
        private static Regex _levelRegex = new(@"([^/]*/)*([\w\d\-]*)\.unity");

        public static string Humanize(this string path)
        {
            return _levelRegex.Replace(path, "$2").AddSpaces();
        }

        public static string ToRomanNumeral(this int number)
        {
            if (number < 0 || number > 3999) throw new ArgumentOutOfRangeException($"{nameof(number)} must be between 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRomanNumeral(number - 1000);
            if (number >= 900) return "CM" + ToRomanNumeral(number - 900);
            if (number >= 500) return "D" + ToRomanNumeral(number - 500);
            if (number >= 400) return "CD" + ToRomanNumeral(number - 400);
            if (number >= 100) return "C" + ToRomanNumeral(number - 100);
            if (number >= 90) return "XC" + ToRomanNumeral(number - 90);
            if (number >= 50) return "L" + ToRomanNumeral(number - 50);
            if (number >= 40) return "XL" + ToRomanNumeral(number - 40);
            if (number >= 10) return "X" + ToRomanNumeral(number - 10);
            if (number >= 9) return "IX" + ToRomanNumeral(number - 9);
            if (number >= 5) return "V" + ToRomanNumeral(number - 5);
            if (number >= 4) return "IV" + ToRomanNumeral(number - 4);
            if (number >= 1) return "I" + ToRomanNumeral(number - 1);
            throw new InvalidOperationException();
        }

        public static string GenerateIncrementalName(IEnumerable<string> existingNames, string baseName)
        {
            var existingNamesAr = existingNames.ToArray();
            var i = 1;
            var name = $"{baseName} {i}";
            while (existingNamesAr.Contains(name))
            {
                i++;
                name = $"{baseName} {i}";
            }

            return name;
        }

        public static string AddSpaces(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            text = text.Replace("_", " ");

            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static string Pluralize(this string text)
        {
            if (text.EndsWith("y", StringComparison.OrdinalIgnoreCase))
            {
                return $"{text.Substring(0, text.Length - 1)}ies";
            }

            return $"{text}s";
        }

        public static string FixPunctuation(this string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            text = text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            if (!text.EndsWith('.') && !text.EndsWith('?') && !text.EndsWith('!') && !text.EndsWith('\"'))
            {
                return $"{text}.";
            }

            return text;
        }
    }
}
