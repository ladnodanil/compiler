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

        public List<Token> Tokens = new List<Token>();


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


            string value = "";
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
                                break;
                        }
                        break;
                    case 1:
                        if ((Char >= 'A' && Char <= 'Z') || (Char >= 'a' && Char <= 'z') || char.IsDigit(Char) || Char == '_')
                        {
                            value += Char;
                            position++;
                        }
                        else
                        {
                            endPosition = position - 1;
                            if (KeyWords.Contains(value))
                            {
                                Tokens.Add(new Token(TypeToken.KEYWORD, value, (beginPosition, endPosition)));
                                if (value == "new")
                                {
                                    if (position < CodeText.Length && CodeText[position] == ' ')
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
                        Tokens.Add(new Token(TypeToken.ERROR, Char.ToString(), (position, position)));
                        position++;
                        Status = 0;
                        break;
                    case 11:
                        Tokens.Add(new Token(TypeToken.END, Char.ToString(), (position, position)));
                        endFound = true;
                        break;
                    default:
                        break;
                }
            }

            return Tokens;
        }
    }
}
