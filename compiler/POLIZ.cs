using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace compiler
{
    public class POLIZ
    {
        public List<Token> Tokens = new List<Token>();

        public Stack<Token> stack = new Stack<Token>();

        public List<Token> outToken = new List<Token>();

        public Stack<double> stackNum = new Stack<double>();

        public POLIZ(List<Token> tokens)
        {
            Tokens = tokens;
        }
        public void ConvertToPOLIZ()
        {
            foreach (Token token in Tokens)
            {
                if(token.Type == TypeToken.NUMBER)
                {
                    outToken.Add(token);
                }
                else
                {
                    if (token.Type == TypeToken.MINUS || token.Type == TypeToken.PLUS || token.Type == TypeToken.MULTIPLICATION || token.Type == TypeToken.DIVISION)
                    {
                        if (stack.Count == 0)
                        {
                            stack.Push(token);
                        }
                        else if (GetPriority(stack.Peek().Value) < GetPriority(token.Value))
                        {
                            stack.Push(token);
                        }
                        else
                        {
                            while (stack.Count > 0 && (GetPriority(stack.Peek().Value) >= GetPriority(token.Value)))
                            {
                                outToken.Add(stack.Pop());
                            }
                            stack.Push(token);
                        }
                    }
                    if (GetPriority(token.Value) == 0)
                    {
                        stack.Push(token);
                    }
                    if (GetPriority(token.Value) == 1)
                    {
                        Token op = stack.Pop();
                        while (stack.Count > 0 && GetPriority(op.Value) != 0)
                        {
                            outToken.Add(op);
                            op = stack.Pop();
                        }
                    }
                }
                
            }
            while (stack.Count > 0)
            {
                outToken.Add(stack.Pop());
            }
        }

        public double Calculate(TypeToken typeToken,double a , double b)
        {
            double res = 0;
            switch (typeToken)
            {
                case TypeToken.MINUS:
                    res = b-a;
                    break;
                case TypeToken.PLUS:
                    res = a+b;
                    break;
                case TypeToken.MULTIPLICATION:
                    res = a*b;
                    break;
                case TypeToken.DIVISION:
                    res = b/a;
                    break;
            }
            return res;
        }
        public double CalculatePOLIZ()
        {
            foreach (var token in outToken)
            {
                if (token.Type == TypeToken.NUMBER)
                {
                    stackNum.Push(double.Parse(token.Value));
                }
                if (token.Type == TypeToken.MINUS || token.Type == TypeToken.PLUS || token.Type == TypeToken.MULTIPLICATION || token.Type == TypeToken.DIVISION)
                {
                    stackNum.Push(Calculate(token.Type,stackNum.Pop(),stackNum.Pop()));
                }
            }
            return stackNum.Peek();
        }

        private int GetPriority(string value)
        {
            switch (value)
            {
                case "(":
                    return 0;
                case ")":
                    return 1;
                case "+":
                    return 7;
                case "-":
                    return 7;
                case "*":
                    return 8;
                case "/":
                    return 8;
                default:
                    return 0;
            }
        }
    }
}
