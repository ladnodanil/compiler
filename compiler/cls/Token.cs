using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler
{
    public enum TypeToken
    {
        PLUS,//+
        MINUS,// -
        MULTIPLICATION,//*
        DIVISION, // /
        OPEN_PARENTHESIS, // круглая скобка
        CLOSE_PARENTHESIS,
        NUMBER,
        ERROR,
        END
    }
    public  class Token
    {
        public TypeToken Type {get; set; }

        public string Value { get; set; }

        public (int,int) Position { get; set; }

        public Token(TypeToken type, string value, (int, int) position)
        {
            Type = type;
            Value = value;
            Position = position;
        }
    }
}
