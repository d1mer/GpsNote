using GpsNote.Enums;
using System.Text.RegularExpressions;

namespace GpsNote.Helpers
{
    public class Validator
    {
        #region -- Patterns regular expressions --

        private static readonly string _patternName = @"^[A-Z]\w{3,15}$";

        private static readonly string _patternEmail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

        #endregion

        public static bool Validate(string entity, VerifyEntity verify)
        {
            bool result = false;

            switch (verify)
            {
                case VerifyEntity.Password:
                    Regex hasNumber = new Regex(@"[0-9]+");
                    Regex hasUpperLetter = new Regex(@"[A-Z]+");
                    Regex hasLowerLetter = new Regex(@"[a-z]+");
                    Regex hasRequeriedLength = new Regex(@".{8,16}");

                    result = hasNumber.IsMatch(entity) &&
                           hasUpperLetter.IsMatch(entity) &&
                           hasLowerLetter.IsMatch(entity) &&
                           hasRequeriedLength.IsMatch(entity);
                    break;
                case VerifyEntity.Email:
                    result = Regex.IsMatch(entity, _patternEmail, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    break;
                case VerifyEntity.Name:
                    result = Regex.IsMatch(entity, _patternName, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    break;
            }

            return result;
        }
    }
}