using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler
{
    public class Error
    {
        public string Message { get; set; }
        public string Fragment { get; set; }
        public (int start, int end) Position { get; set; }

        public Error(string message, string fragment, (int start, int end) position)
        {
            Message = message;
            Fragment = fragment;
            Position = (position.start, position.end + 1);
        }
    }
}
