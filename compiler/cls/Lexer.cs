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

        public List<string> move = new List<string>();

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
            while (Char != '\0')
            {

                Char = position < CodeText.Length ? CodeText[position] : '\0';

                switch (Status)
                {
                    case 0:
                        move.Add("START");
                        switch (Char)
                        {
                            case char c when char.IsLetter(c) && c >= 65 && c <= 122:
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

                            default:
                                Status = 10;
                                break;
                        }
                        break;
                    case 1:
                        move.Add("1");
                        if (char.IsLetterOrDigit(Char) || (Char >= 65 && Char <= 122))
                        {
                            value += Char;
                            position++;
                        }
                        else
                        {
                            endPosition = position-1;
                            if (KeyWords.Contains(value))
                            {
                                int code = 6;
                                switch (value)
                                {
                                    case "int":
                                        code = 1;
                                        break;
                                    case "string":
                                        code = 2;
                                        break;
                                    case "new":
                                        code = 3;
                                        break;
                                    case "Dictionary":
                                        code = 4;
                                        break;
                                }
                                
                                Tokens.Add(new Token(code, TypeToken.KEYWORD, value, (beginPosition, endPosition)));
                                move.Add("OUT");
                            }
                            else
                            {
                                Tokens.Add(new Token(6, TypeToken.ID, value, (beginPosition, endPosition)));
                                move.Add("OUT");
                            }
                            value = "";
                            Status = 0;
                            
                        }
                        break;
                    case 2:
                        move.Add("2");
                        Tokens.Add(new Token(7, TypeToken.DELIMETER, Char.ToString(), (position, position )));
                        move.Add("OUT");
                        position++;
                        Status = 0;
                        break;
                    case 3:
                        move.Add("3");
                        Tokens.Add(new Token(8, TypeToken.OPERATOR_COMPARSION, Char.ToString(), (position, position)));
                        move.Add("OUT");
                        position++;
                        Status = 0;
                        break;
                    case 4:
                        move.Add("4");
                        Tokens.Add(new Token(9, TypeToken.OPERATOR_COMPARSION, Char.ToString(), (position, position)));
                        move.Add("OUT");
                        position++;
                        Status = 0;
                        break;
                    case 5:
                        move.Add("5");
                        Tokens.Add(new Token(10, TypeToken.PARENTHESIS, Char.ToString(), (position, position)));
                        move.Add("OUT");
                        position++;
                        Status = 0;
                        break;
                    case 6:
                        move.Add("6");
                        Tokens.Add(new Token(11, TypeToken.PARENTHESIS, Char.ToString(), (position, position)));
                        move.Add("OUT");
                        position++;
                        Status = 0;
                        break;
                    case 7:
                        move.Add("7");
                        Tokens.Add(new Token(12, TypeToken.COMMA, Char.ToString(), (position, position)));
                        move.Add("OUT");
                        position++;
                        Status = 0;
                        break;
                    case 8:
                        move.Add("8");
                        Tokens.Add(new Token(13, TypeToken.OPERATOR_END, Char.ToString(), (position, position)));
                        move.Add("OUT");
                        position++;
                        Status = 0;
                        break;
                    case 9:
                        move.Add("9");
                        Tokens.Add(new Token(14, TypeToken.OPERATOR_ASSIGNMENT, Char.ToString(), (position, position)));
                        move.Add("OUT");
                        position++;
                        Status = 0;
                        break;
                    case 10:
                        move.Add("10");
                        Tokens.Add(new Token(15, TypeToken.ERROR, Char.ToString(), (position, position)));
                        return Tokens;
                    default:
                        break;
                }
            }

            return Tokens;
        }
    }
}
