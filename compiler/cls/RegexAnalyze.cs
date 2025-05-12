using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace compiler
{
    public class RegexAnalyze
    {
        public Regex Regex;

        private string text;

        public MatchCollection Matches;
        public RegexAnalyze(string text, string pattern)
        {
            Regex = new Regex(pattern);
            this.text = text;
            Matches = Regex.Matches(text);

        }
        public List<MatchInfo> GetMatchInfoList()
        {
            var matchInfoList = new List<MatchInfo>();
            foreach (Match match in Matches)
            {
                matchInfoList.Add(new MatchInfo
                {
                    Position = (match.Index, match.Index + match.Length-1),
                    Value = match.Value
                });
            }
            return matchInfoList;
        }
    }
    public class MatchInfo
    {
        public string Value { get; set; }
        public (int,int) Position { get; set; }
    }
}
