using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace YukinoshitaBot.Extensions
{
    public static class RegexExtension
    {
        public static bool TryGetMatchPairs(this Regex regex, in string input, [NotNullWhen(true)] out Dictionary<string, string> matchPairs)
        {
            var matchs = regex.Match(input);
            if (matchs.Success == false)
            {
                matchPairs = new Dictionary<string, string>();
                return false;
            }
            var groups = matchs.Groups.Values;
            matchPairs = (from item in groups
                          where !string.IsNullOrEmpty(item.Name) && !string.IsNullOrWhiteSpace(item.Value)
                          select item)
                         .ToDictionary(t => t.Name, t => t.Value);
            return true;
        }
    }
}
