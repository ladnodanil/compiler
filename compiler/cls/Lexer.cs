using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace compiler
{

    public class Lexer
    {
        public string CodeText;

        public int State;
        public int prevStatus;


        public List<Token> Tokens = new List<Token>();
        public List<Error> Errors = new List<Error>();


        public Lexer(string Text)
        {
            this.CodeText = Text.Replace("\n", " ").Replace("\t", " ").Replace("\r", " ");
        }

        private void AddToken(TypeToken type, string value, (int,int) position)
        {
            Tokens.Add(new Token(type,value,position));
        }

        public List<Token> Analyze()
        {
            int position = 0;
            int beginPosition = 0;
            int endPosition = 0;
            char Char = ' ';
            int errorStart = 0;
            string number = "";
            string errorFragment = "";
            bool endFound = false;
            while (!endFound)
            {
                Char = position < CodeText.Length ? CodeText[position] : '\0';
                switch (State)
                {
                    case 0:
                        switch (Char)
                        {
                            case char c when char.IsDigit(Char):
                                State = 1;
                                beginPosition = position;
                                break;
                            case '+':
                                State = 2;
                                break;
                            case '-':
                                State = 3;
                                break;
                            case '*':
                                State = 4;
                                break;
                            case '/':
                                State = 5;
                                break;
                            case '(':
                                State = 6;
                                break;
                            case ')':
                                State = 7;
                                break;
                            case ' ':
                                position++;
                                break;
                            case '\0':
                                State = 8;
                                break;
                            default:
                                State = 9;
                                errorFragment += Char;
                                errorStart = position;
                                position++;
                                break;
                        }
                        break;
                    case 1:
                        if (char.IsDigit(Char))
                        {
                            number += Char;
                            position++;

                        }
                        else
                        {
                            endPosition = position-1;
                            AddToken(TypeToken.NUMBER, number, (beginPosition, endPosition));
                            State = 0;
                            number = "";

                        }
                        break;
                    case 2:
                        AddToken(TypeToken.PLUS, Char.ToString(), (position, position));
                        State = 0;
                        position++;
                        break;
                    case 3:
                        AddToken(TypeToken.MINUS, Char.ToString(), (position, position));
                        State = 0;
                        position++;
                        break;
                    case 4:
                        AddToken(TypeToken.MULTIPLICATION, Char.ToString(), (position, position));
                        State = 0;
                        position++;
                        break;
                    case 5:
                        AddToken(TypeToken.DIVISION, Char.ToString(), (position, position));
                        State = 0;
                        position++;
                        break;
                    case 6:
                        AddToken(TypeToken.OPEN_PARENTHESIS, Char.ToString(), (position, position));
                        State = 0;
                        position++;
                        break;
                    case 7:
                        AddToken(TypeToken.CLOSE_PARENTHESIS, Char.ToString(), (position, position));
                        State = 0;
                        position++;
                        break;
                    case 9:
                        if (position < CodeText.Length && IsError(Char))
                        {
                            errorFragment += Char;
                            position++;
                        }
                        else
                        {
                            Errors.Add(new Error("Ошибочный фрагмент", errorFragment, (errorStart, position - 1)));
                            errorFragment = "";
                            State = prevStatus;
                        }
                        break;
                    case 8:
                        endFound = true;
                        break;
                    default:
                        break;
                }
                if (State != 9)
                {
                    prevStatus = State;
                }

            }

            return Tokens;
        }

        private bool IsError(char c)
        {
            if (!(char.IsDigit(c) 
                || c == '+'
                || c == '-'
                || c == '*'
                || c == '/'
                || c == '('
                || c == ')'
                ))
            {
                return true;
            }
            return false;
        }

    }
}
