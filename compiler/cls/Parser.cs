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

        public List<Error> Errors = new List<Error>();

        public List<string> Log = new List<string>();

        public Lexer Lexer;


        public Parser(string codeText)
        {
            Lexer = new Lexer(codeText);
            Lexer.Analyze();
            Tokens = Lexer.Tokens;
        }
        private void handleError(string message, string value, (int, int) pos)
        {
            Errors.Add(new Error(message, value, pos));
            this.pos++;

        }
        public void Parse()
        {
            While();
        }

        public void While()
        {
            Log.Add("While");
            if (pos < Tokens.Count && Tokens[pos].Type == TypeToken.WHILE)
            {
                Log.Add(Tokens[pos].Value);
                pos++;
                Cond();
                if (pos < Tokens.Count && Tokens[pos].Type == TypeToken.DO)
                {
                    Log.Add(Tokens[pos].Value);
                    pos++;
                    Stmt();
                    if (pos < Tokens.Count && Tokens[pos].Type == TypeToken.END)
                    {
                        Log.Add(Tokens[pos].Value);
                        pos++;
                        While();
                    }
                    else
                    {
                        handleError("Ожидался конец ; ", Tokens[pos].Value, Tokens[pos].Position);
                    }
                }
                else
                {
                    handleError("Ожидалось ключевое слово do", Tokens[pos].Value, Tokens[pos].Position);
                    Stmt();
                }
            }
            else
            {
                handleError("Ожидалось ключевое слово while", Tokens[pos].Value, Tokens[pos].Position);
                Cond();
            }
        }

        public void Cond()
        {
            Log.Add("Cond");
            LogExpr();
            while (pos < Tokens.Count &&  Tokens[pos].Type == TypeToken.OR)
            {
                Log.Add(Tokens[pos].Value);
                pos++;
                LogExpr();
            }
        }

        public void LogExpr()
        {
            Log.Add("LogExpr");
            RelExpr();
            while (pos < Tokens.Count && Tokens[pos].Type == TypeToken.AND)
            {
                Log.Add(Tokens[pos].Value);
                pos++;
                LogExpr();
            }
        }

        public void RelExpr()
        {
            Log.Add("RelExpr");
            Operand();
            if(pos < Tokens.Count && Tokens[pos].Type == TypeToken.REL)
            {
                Log.Add(Tokens[pos].Value);
                pos++;
                Operand();
            }
            else
            {
                handleError("Ожидалась операция сравнения", Tokens[pos].Value, Tokens[pos].Position);
                Operand();
            }
        }

        public void Operand()
        {
            Log.Add("Operand");
            if (pos < Tokens.Count && (Tokens[pos].Type == TypeToken.VAR || Tokens[pos].Type == TypeToken.CONST))
            {
                Log.Add(Tokens[pos].Value);
                pos++;
            }
            else
            {
                handleError("Ожидалось число|переменная", Tokens[pos].Value, Tokens[pos].Position);
            }
        }

        public void Stmt()
        {
            Log.Add("Stmt");
            if (pos < Tokens.Count && Tokens[pos].Type == TypeToken.VAR)
            {
                Log.Add(Tokens[pos].Value);
                pos++;
                if (pos < Tokens.Count && Tokens[pos].Type == TypeToken.AS)
                {
                    Log.Add(Tokens[pos].Value);
                    pos++;
                    ArithExpr();
                }
                else
                {
                    handleError("Ожидался оператор присваивания", Tokens[pos].Value, Tokens[pos].Position);
                    ArithExpr();
                }
            }
            else
            {
                handleError("Ожидалось переменная", Tokens[pos].Value, Tokens[pos].Position);
            }
        }

        public void ArithExpr()
        {
            Log.Add("ArithExpr");
            Operand();
            while (pos < Tokens.Count && Tokens[pos].Type == TypeToken.AO)
            {
                Log.Add(Tokens[pos].Value);
                pos++;
                Operand();
            }
        }




    }
}
