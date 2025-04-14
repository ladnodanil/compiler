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

        public int Status;
        public int prevStatus;

        public List<Token> Tokens = new List<Token>();
        public List<Error> Errors = new List<Error>();


        public List<string> KeyWords = new List<string>
        {
            "int",
            "string",
            "new",
            "Dictionary",
        };

        public Lexer(string Text)
        {
            this.CodeText = Text.Replace("\n", " ").Replace("\t", " ").Replace("\r", " ");
        }

        public List<Token> Analyze()
        {
            int position = 0;
            int beginPosition = 0;
            int endPosition = 0;
            char Char = ' ';

            int errorStart = 0;
            string value = "";
            string errorFragment = "";
            bool endFound = false;
            while (!endFound)
            {
                Char = position < CodeText.Length ? CodeText[position] : '\0';
                switch (Status)
                {
                    case 0:
                        switch (Char)
                        {
                            case char c when (Char >= 'A' && Char <= 'Z') || (Char >= 'a' && Char <= 'z'):
                                value += c;
                                beginPosition = position;
                                Status = 1;
                                position++;
                                break;

                            case ' ':
                                position++;
                                break;

                            case '<':
                                Status = 3;
                                break;

                            case '>':
                                Status = 4;
                                break;

                            case '(':
                                Status = 5;
                                break;

                            case ')':
                                Status = 6;
                                break;

                            case ',':
                                Status = 7;
                                break;

                            case ';':
                                Status = 8;
                                break;

                            case '=':
                                Status = 9;
                                break;
                            case '\0':
                                Status = 11;
                                break;
                            default:
                                Status = 10;
                                errorFragment += Char;
                                errorStart = position;
                                position++;
                                break;
                        }
                        break;
                    case 1:
                        if ((Char >= 'A' && Char <= 'Z') || (Char >= 'a' && Char <= 'z') || char.IsDigit(Char) || Char == '_')
                        {
                            value += Char;
                            position++;
                        }
                        else if (IsError(Char))
                        {
                            errorStart = position;
                            Status = 10;
                        }
                        else
                        {
                            endPosition = position - 1;
                            if (KeyWords.Contains(value))
                            {
                                Tokens.Add(new Token(TypeToken.KEYWORD, value, (beginPosition, endPosition)));
                                if (value == "new")
                                {
                                    if (position < CodeText.Length && Char == ' ')
                                    {
                                        Status = 2;
                                    }
                                    else
                                    {
                                        Status = 10;
                                    }
                                }
                                else
                                {
                                    Status = 0;
                                }
                            }
                            else
                            {
                                Tokens.Add(new Token(TypeToken.ID, value, (beginPosition, endPosition)));
                                Status = 0;
                            }
                            value = "";
                        }
                        break;
                    case 2:
                        Tokens.Add(new Token(TypeToken.DELIMETER, Char.ToString(), (position, position)));
                        position++;
                        Status = 0;
                        break;
                    case 3:
                        Tokens.Add(new Token(TypeToken.OPERATOR_COMPARSION, Char.ToString(), (position, position)));
                        position++;
                        Status = 0;
                        break;
                    case 4:
                        Tokens.Add(new Token(TypeToken.OPERATOR_COMPARSION, Char.ToString(), (position, position)));
                        position++;
                        Status = 0;
                        break;
                    case 5:
                        Tokens.Add(new Token(TypeToken.PARENTHESIS, Char.ToString(), (position, position)));
                        position++;
                        Status = 0;
                        break;
                    case 6:
                        Tokens.Add(new Token(TypeToken.PARENTHESIS, Char.ToString(), (position, position)));
                        position++;
                        Status = 0;
                        break;
                    case 7:
                        Tokens.Add(new Token(TypeToken.COMMA, Char.ToString(), (position, position)));
                        position++;
                        Status = 0;
                        break;
                    case 8:
                        Tokens.Add(new Token(TypeToken.OPERATOR_END, Char.ToString(), (position, position)));
                        position++;
                        Status = 0;
                        break;
                    case 9:
                        Tokens.Add(new Token(TypeToken.OPERATOR_ASSIGNMENT, Char.ToString(), (position, position)));
                        position++;
                        Status = 0;
                        break;
                    case 10:
                        if (position < CodeText.Length && IsError(Char))
                        {
                            errorFragment += Char;
                            position++;
                        }
                        else
                        {
                            Errors.Add(new Error("Ошибочный фрагмент", errorFragment, (errorStart, position - 1)));
                            errorFragment = "";
                            Status = prevStatus;
                        }

                        break;
                    case 11:
                        Tokens.Add(new Token(TypeToken.END, Char.ToString(), (position, position)));
                        endFound = true;
                        break;
                    default:
                        break;
                }
                if (Status != 10)
                {
                    prevStatus = Status;
                }
            }

            return Tokens;
        }

        private bool IsError(char c)
        {
            if (!((c >= 'A' && c <= 'Z') ||
                (c >= 'a' && c <= 'z') ||
                char.IsDigit(c) ||
                c == '_' ||
                c == ' ' ||
                c == '<' ||
                c == '>' ||
                c == '(' ||
                c == ')' ||
                c == ',' ||
                c == ';' ||
                c == '=') && c != '\0')
            {
                return true;
            }
            return false;
        }
    }
}
