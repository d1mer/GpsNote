using System.Text.RegularExpressions;

namespace GpsNote.Helpers
{
    public class Validator
    {
        #region -- Patterns regular expressions --

        public static readonly string patternName = @"^[A-Z]\w{3,15}$";

        public static readonly string patternEmail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

        public static readonly string patternPassword = null;

        #endregion

        public static bool Validate(string entity, string pattern)
        {
            if(pattern == null)
            {
                Regex hasNumber = new Regex(@"[0-9]+");
                Regex hasUpperLetter = new Regex(@"[A-Z]+");
                Regex hasLowerLetter = new Regex(@"[a-z]+");
                Regex hasRequeriedLength = new Regex(@".{8,16}");

                return hasNumber.IsMatch(entity) && 
                       hasUpperLetter.IsMatch(entity) && 
                       hasLowerLetter.IsMatch(entity) && 
                       hasRequeriedLength.IsMatch(entity);
            }
            return Regex.IsMatch(entity, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
    }
}