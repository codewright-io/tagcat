using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        public static string OfuscatePasswords(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return source;

            var match = Regex.Match(source, @"(pwd|password)=.*?(;|$|\z)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var splits = match.Value.Split('=');
                int passwordLength = splits[1].Length;
                string blankedPassword = passwordLength <= 0 ? "{none}" : new string('*', passwordLength);
                source = source.Replace(splits[1], $"{blankedPassword};", StringComparison.InvariantCulture);
            }

            return source;
        }
    }
}
