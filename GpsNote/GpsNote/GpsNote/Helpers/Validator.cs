using System.Text.RegularExpressions;

namespace GpsNote.Helpers
{
    public class Validator
    {
        #region -- Patterns regular expressions --

        public readonly string patternName = @"^[A-Z]\w{3,15}$";

        public readonly string patternEmail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

        public readonly string patternPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{8,16}$";

        #endregion

        public static bool Validate(string entity, string pattern) =>
            Regex.IsMatch(entity, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}