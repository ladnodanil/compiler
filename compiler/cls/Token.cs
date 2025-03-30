using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler
{
    public enum TypeToken
    {
        ID, 
        KEYWORD,
        DELIMETER, //разделитель
        OPERATOR_COMPARSION, // оператор сравнения
        PARENTHESIS, // круглая скобка
        COMMA, // запятая
        OPERATOR_END,
        OPERATOR_ASSIGNMENT, // оператор присваивания
        ERROR
    }
    public  class Token
    {
        public int Code { get; set; }
        public TypeToken Type {get; set; }

        public string Value { get; set; }

        public (int,int) Position { get; set; }

        public Token(int code, TypeToken type, string value, (int, int) position)
        {
            Code = code;
            Type = type;
            Value = value;
            Position = position;
        }
    }
}
