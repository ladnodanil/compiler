using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace compiler
{
    public class Lexer
    {
        private string codeText;

        public List<Token> Tokens = new List<Token>();
        public List<Error> Errors = new List<Error>();
        public int State;
        public int prevStatus;
        public Lexer(string codeText)
        {
            this.codeText = codeText.Replace("\n", " ").Replace("\t", " ").Replace("\r", " ");

        }
        private void AddToken(TypeToken type, string value, (int, int) position)
        {
            Tokens.Add(new Token(type, value, position));
        }
        public void Analyze()
        {
            List<string> keyWord = new List<string>() {
                "while", "do", "and", "or"
            };

            int position = 0;
            int beginPosition = 0;
            int endPosition 
                = 0;
            char Char = ' ';
            string value = "";

            int errorStart = 0;
            string errorFragment = "";

            bool endFound = false;

            while (!endFound)
            {
                Char = position < codeText.Length ? codeText[position] : '\0';

                switch (State)
                {
                    case 0:
                        switch (Char)
                        {
                            case char c when char.IsDigit(Char):
                                State = 1;
                                beginPosition = position;
                                break;
                            case char c when char.IsLetter(Char):
                                State = 2;
                                beginPosition = position;
                                break;
                            case char c when IsAO(Char):
                                State = 3;
                                break;
                            case char c when IsRel(Char):
                                State = 4;
                                beginPosition = position;
                                break;
                            case '\0':
                                State = 5;
                                break;
                            case ' ':
                                position++;
                                break;
                            case ';':
                                State = 7;
                                break;
                            default:
                                State = 6;
                                errorFragment += Char;
                                errorStart = position;
                                position++;
                                break;
                        }
                        break;
                    case 1:
                        if (char.IsDigit(Char))
                        {
                            value += Char;
                            position++;
                        }
                        else if (IsError(Char))
                        {
                            errorStart = position;
                            State = 6;
                        }
                        else
                        {
                            endPosition = position - 1;
                            AddToken(TypeToken.CONST, value, (beginPosition, endPosition));
                            State = 0;
                            value = "";
                        }
                        break;
                    case 2:
                        if (char.IsLetter(Char))
                        {
                            value += Char;
                            position++;
                        }
                        else if (IsError(Char))
                        {
                            errorStart = position;
                            State = 6;
                        }
                        else
                        {
                            endPosition = position - 1;
                            if (keyWord.Contains(value))
                            {
                                switch (value)
                                {
                                    case "while":
                                        AddToken(TypeToken.WHILE, value, (beginPosition, endPosition));
                                        break;
                                    case "do":
                                        AddToken(TypeToken.DO, value, (beginPosition, endPosition));
                                        break;
                                    case "and":
                                        AddToken(TypeToken.AND, value, (beginPosition, endPosition));
                                        break;
                                    case "or":
                                        AddToken(TypeToken.OR, value, (beginPosition, endPosition));
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                AddToken(TypeToken.VAR, value, (beginPosition, endPosition));
                            }
                            State = 0;
                            value = "";
                        }
                        break;
                    case 3:
                        AddToken(TypeToken.AO, Char.ToString(), (position, position));
                        State = 0;
                        position++;
                        break;
                    case 4:
                        if (IsRel(Char))
                        {
                            value += Char;
                            position++;
                        }
                        else if (IsError(Char))
                        {
                            errorStart = position;
                            State = 6;
                        }
                        else
                        {
                            endPosition = position - 1;
                            switch (value)
                            {
                                case "<":
                                    AddToken(TypeToken.REL, value, (beginPosition, endPosition));
                                    break;
                                case "<=":
                                    AddToken(TypeToken.REL, value, (beginPosition, endPosition));
                                    break;
                                case ">=":
                                    AddToken(TypeToken.REL, value, (beginPosition, endPosition));
                                    break;
                                case ">":
                                    AddToken(TypeToken.REL, value, (beginPosition, endPosition));
                                    break;
                                case "!=":
                                    AddToken(TypeToken.REL, value, (beginPosition, endPosition));
                                    break;
                                case "==":
                                    AddToken(TypeToken.REL, value, (beginPosition, endPosition));
                                    break;
                                case "=":
                                    AddToken(TypeToken.AS, value, (beginPosition, endPosition));
                                    break;
                                default:
                                    break;
                            }
                            State = 0;
                            value = "";
                        }
                        break;
                    case 5:
                        endFound = true;
                        break;
                    case 6:
                        if (position < codeText.Length && IsError(Char))
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
                    case 7:
                        AddToken(TypeToken.END, Char.ToString(), (position, position));
                        State = 0;
                        position++;
                        break;
                    default:
                        break;
                }
                if (State != 6)
                {
                    prevStatus = State;
                }
            }


        }

        private bool IsError(char c)
        {
            if (!(char.IsDigit(c)
                || c == '+'
                || c == '-'
                || c == '>'
                || c == '='
                || c == '<'
                || c == '!'
                || c == ' '
                || c == '\0'
                || c == ';'
                || char.IsLetter(c)
                ))
            {
                return true;
            }
            return false;
        }

        private bool IsAO(char Char)
        {
            string str = "+-";
            return str.Contains(Char);
        }

        private bool IsRel(char Char)
        {
            string str = "<=>!";
            return str.Contains(Char);
        }

    }
}
