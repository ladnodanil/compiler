﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler
{
    public class ParseError:Exception
    {
        public string ErrorValue;

        public int startPos;
        public int endPos;
        public ParseError(string message, string value,(int,int) pos): base(message)
        {
            ErrorValue = value;
            startPos = pos.Item1;
            endPos = pos.Item2+1;
        }
    }
}
