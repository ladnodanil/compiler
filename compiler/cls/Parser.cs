using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler
{
    public class Parser
    {
        private int pos = 0;

        public List<Token> Tokens = new List<Token>();

        public List<Error> errors = new List<Error>();

        public Lexer Lexer;

        
        public Parser(string codeText)
        {
            Lexer = new Lexer(codeText);
            Lexer.Analyze();
            Tokens = Lexer.Tokens;
        }

        private void handleError(string message, string value, (int, int) pos)
        {
            errors.Add(new Error(message, value, pos));

        }

        public void Parse()
        {
            E();
        }

        public void E() //  E → TA 
        {
            T();
            A();
        }
        public void T() // T → ОВ 
        {
            O();
            B();
        }
        public void A() // A → ε | + TA | - TA 
        {
            if (pos < Tokens.Count && (Tokens[pos].Type == TypeToken.PLUS || Tokens[pos].Type == TypeToken.MINUS))
            {
                pos++;
                T();
                A();
            }
            
            
        }
        public void B() // В → ε | *ОВ | /ОВ 
        {
            if (pos < Tokens.Count && (Tokens[pos].Type == TypeToken.MULTIPLICATION || Tokens[pos].Type == TypeToken.DIVISION))
            {
                pos++;
                O();
                B();
            }
        }
        public void O() // О → num | (E) 
        {
            if (pos >= Tokens.Count)
            {
                handleError("Ожидался операнд, но достигнут конец ввода", "", (Tokens.Last().Position.Item2, Tokens.Last().Position.Item2));
                return;
            }
            if (pos < Tokens.Count)
            {
                if (Tokens[pos].Type == TypeToken.NUMBER)
                {
                    pos++;
                }
                else if (Tokens[pos].Type == TypeToken.OPEN_PARENTHESIS)
                {
                    pos++;
                    E();
                    if (pos < Tokens.Count && Tokens[pos].Type == TypeToken.CLOSE_PARENTHESIS)
                    {
                        pos++;
                    }
                    else
                    {
                        handleError("Ожидалась закрывающая скобка", Tokens[pos-1].Value, Tokens[pos-1].Position);
                    }
                }
                else
                {
                    handleError("Ожидался операнд", Tokens[pos].Value, Tokens[pos].Position);
                }
            }
        }
    }
}

