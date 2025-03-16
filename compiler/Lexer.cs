using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace compiler
{
    
    public class Lexer
    {
        public string CodeText;

        public List<string> KeyWords = new List<string>
        {
            "int",
            "string",
            "new",
            "Dictionary",
        };

        public Lexer(string Text)
        {
            this.CodeText = Text.Replace("\n","").Replace("\t","");
        }

        
        public  DataTable Analyze()
        {
            int status = 0;
            DataTable data = new DataTable();
            data.Columns.Add("Код");
            data.Columns.Add("Тип");
            data.Columns.Add("Значение");
            data.Columns.Add("Место");
            string word = "";
            int pos = 0;
            int beginPos = 0;
            while (pos < CodeText.Length)
            {
                char ch = CodeText[pos];
                switch (status)
                {
                    case 0:
                        if (char.IsLetter(CodeText[pos]))
                        {
                            word += CodeText[pos];
                            beginPos = pos;
                            status = 1;
                            pos++;
                        }
                        else if (CodeText[pos] == ' ')
                        {
                            pos++;
                        }
                        else if (CodeText[pos] == '<')
                        {
                            status = 3;
                        }
                        else if (CodeText[pos] == '>')
                        {
                            status = 4;
                        }
                        else if (CodeText[pos] == '(')
                        {
                            status = 5;
                        }
                        else if (CodeText[pos] == ')')
                        {
                            status = 6;
                        }
                        else if (CodeText[pos] == ',')
                        {
                            status = 7;
                        }
                        else if (CodeText[pos] == ';')
                        {
                            status = 8;
                        }
                        else if (CodeText[pos] == '=')
                        {
                            status = 9;
                        }
                        break;
                    case 1:
                        if (char.IsLetterOrDigit(CodeText[pos]) || CodeText[pos] == '_')
                        {
                            word += CodeText[pos];
                            pos++;
                        }
                        else
                        {
                            if (KeyWords.Contains(word))
                            {
                                switch (word)
                                {
                                    case "int":
                                        data.Rows.Add(1, "ключевое слово", word, $"с {beginPos + 1} по {pos + 1}");
                                        break;
                                    case "string":
                                        data.Rows.Add(2, "ключевое слово", word, $"с {beginPos + 1} по {pos + 1}");
                                        break;
                                    case "new":
                                        data.Rows.Add(3, "ключевое слово", word, $"с {beginPos + 1} по {pos + 1}");
                                        break;
                                    case "Dictionary":
                                        data.Rows.Add(4, "ключевое слово", word, $"с {beginPos + 1} по {pos + 1}");
                                        break;
                                    default:
                                        break;
                                }
                                
                            }
                            else
                            {
                                data.Rows.Add(5, "идентификатор", word, $"с {beginPos + 1} по {pos + 1}");
                            }
                            word = "";
                            status = 0;
                            if (pos < CodeText.Length && char.IsWhiteSpace(CodeText[pos]))
                            {
                                status = 2;
                            }
                        }

                        break;
                    case 2:
                        data.Rows.Add(6, "разделитель", "пробел", $"с {pos + 1} по {pos + 1}");
                        status = 0;
                        pos++;
                        break;
                    case 3:
                        data.Rows.Add(7, "оператор сравнения", CodeText[pos].ToString(), $"с {pos + 1} по {pos + 1}");
                        status = 0;
                        pos++;
                        break;
                    case 4:
                        data.Rows.Add(8, "оператор сравнения", CodeText[pos].ToString(), $"с {pos + 1} по {pos + 1}");
                        status = 0;
                        pos++;
                        break;
                    case 5:
                        data.Rows.Add(9, "круглая скобка", CodeText[pos].ToString(), $"с {pos + 1} по {pos + 1}");
                        status = 0;
                        pos++;
                        break;
                    case 6:
                        data.Rows.Add(10, "круглая скобка", CodeText[pos].ToString(), $"с {pos + 1} по {pos + 1}");
                        status = 0;
                        pos++;
                        break;
                    case 7:
                        data.Rows.Add(11, "запятая", CodeText[pos].ToString(), $"с {pos + 1} по {pos + 1}");
                        status = 0;
                        pos++;
                        break;
                    case 8:
                        data.Rows.Add(12, "конец оператора", CodeText[pos].ToString(), $"с {pos + 1} по {pos + 1}");
                        status = 0;
                        pos++;
                        break;
                    case 9:
                        data.Rows.Add(13, "оператор присваивания", CodeText[pos].ToString(), $"с {pos + 1} по {pos + 1}");
                        status = 0;
                        pos++;
                        break;
                    default:
                        data.Rows.Add(666, "недопустимый символ", CodeText[pos].ToString(), $"с {pos + 1} по {pos + 1}");
                        status = 0;
                        pos++;
                        break;
                }
                
            }
            return data;

            
        }

    }
}
