using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static string CleanSpacesAndIgnoreUTF8(string value, bool doUpper = true)
        {
            string result = "";

            if (doUpper)
                result = string.Join("", value.ToUpper().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            else
                result = string.Join("", value.ToLower().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

            result = ChangeToEnglish(result);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ChangeToEnglish(string value)
        {
            string result = value;

            result = result.Replace("ü", "u");
            result = result.Replace("ı", "i");
            result = result.Replace("ö", "o");
            result = result.Replace("ü", "u");
            result = result.Replace("ş", "s");
            result = result.Replace("ğ", "g");
            result = result.Replace("ç", "c");

            result = result.Replace("Ü", "U");
            result = result.Replace("İ", "I");
            result = result.Replace("Ö", "O");
            result = result.Replace("Ü", "U");
            result = result.Replace("Ş", "S");
            result = result.Replace("Ğ", "G");
            result = result.Replace("Ç", "C");

            return result;
        }
    }
}
