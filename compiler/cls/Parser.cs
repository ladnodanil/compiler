using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace compiler
{
    public class Parser
    {
        private enum State
        {
            START,
            GENERIC_TYPE,
            TKEY,
            COMMA,
            TVALUE,
            CLOSE_GENERIC,
            ID,
            IDREM,
            NEW,
            SPASE,
            DICT_CREATTION,
            GENERIC_TYPE2,
            TKEY2,
            COMMA2,
            TVALUE2,
            CLOSE_GENERIC2,
            OPEN_PAREN,
            CLOSE_PAREN,
            END,
            ERROR
        }

        private State state = State.START;

        private Token Token;

        private int pos = 0;

        public List<Token> Tokens = new List<Token>();

        public List<Error> errors = new List<Error>();

        public Lexer Lexer;

        public List<string> type = new List<string>
        {
            "int",
            "string",
        };
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
            while (pos < Tokens.Count-1)
            {
                Token = GetToken();
                switch (state)
                {
                    case State.START:
                        stateSTART();
                        break;
                    case State.GENERIC_TYPE:
                        stateGENERIC_TYPE();
                        break;
                    case State.TKEY:
                        stateTKEY();
                        break;
                    case State.COMMA:
                        stateCOMMA();
                        break;
                    case State.TVALUE:
                        stateTVALUE();
                        break;
                    case State.CLOSE_GENERIC:
                        stateCLOSE_GENERIC();
                        break;
                    case State.ID:
                        stateID();
                        break;
                    case State.IDREM:
                        stateIDREM();
                        break;
                    case State.NEW:
                        stateNEW();
                        break;
                    case State.SPASE:
                        stateSPASE();
                        break;
                    case State.DICT_CREATTION:
                        stateDICT_CREATTION();
                        break;
                    case State.GENERIC_TYPE2:
                        stateGENERIC_TYPE2();
                        break;
                    case State.TKEY2:
                        stateTKEY2();
                        break;
                    case State.COMMA2:
                        stateCOMMA2();
                        break;
                    case State.TVALUE2:
                        stateTVALUE2();
                        break;
                    case State.CLOSE_GENERIC2:
                        stateCLOSE_GENERIC2();
                        break;
                    case State.OPEN_PAREN:
                        stateOPEN_PAREN();
                        break;
                    case State.CLOSE_PAREN:
                        stateCLOSE_PAREN();
                        break;
                    case State.END:
                        stateEND();
                        break;
                    default:
                        break;


                }
                pos++;
            }

        }
        private Token GetToken()
        {
            if (pos < Tokens.Count - 1)
            {
                return Tokens[pos];
            }
            return null;
        }
        private Token GetNextToken()
        {

            if (pos < Tokens.Count - 1)
            {
                return Tokens[pos + 1];
            }
            return null;
        }
        
        private void setState(State value)
        {
            state = value;
        }
        private void stateSTART()
        {
            if (Token.Value == "Dictionary")
            {
                setState(State.GENERIC_TYPE);
            }
            else
            {
                if (Token.Value == "<" && type.Contains(GetNextToken().Value))
                {
                    setState(State.TKEY);
                    handleError("Пропущено ключевое слово 'Dictionary'", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {


                    if (GetNextToken().Value == "Dictionary")
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.START);
                    }
                    else
                    {
                        setState(State.GENERIC_TYPE);
                        handleError("Ожидалось ключевое слово 'Dictionary'", Token.Value, Token.Position);
                    }
                }
            }
        }


        private void stateGENERIC_TYPE()
        {
            if (Token.Value == "<")
            {
                setState(State.TKEY);
            }
            else
            {
                if (type.Contains(Token.Value) && GetNextToken().Type == TypeToken.COMMA)
                {
                    setState(State.COMMA);
                    handleError("Пропущен оператор сравнения '<'", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {

                    if (GetNextToken().Value == "<")
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.GENERIC_TYPE);
                    }
                    else
                    {
                        handleError("Ожидался оператор сравнения '<'", Token.Value, Token.Position);
                        setState(State.TKEY);
                    }
                }
            }
        }
        private void stateTKEY()
        {
            if (type.Contains(Token.Value))
            {
                setState(State.COMMA);
            }
            else
            {
                if (Token.Type == TypeToken.COMMA && type.Contains(GetNextToken().Value))
                {
                    setState(State.TVALUE);
                    handleError("Пропущен тип данных ", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));

                }
                else
                {
                    if (type.Contains(GetNextToken().Value))
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.TKEY);
                    }
                    else
                    {
                        handleError("Ожидался тип данных", Token.Value, Token.Position);
                        setState(State.COMMA);

                    }
                }
            }

        }
        private void stateCOMMA()
        {
            if (Token.Type == TypeToken.COMMA)
            {
                setState(State.TVALUE);
            }
            else
            {
                if (type.Contains(Token.Value) && GetNextToken().Value == ">")
                {
                    setState(State.CLOSE_GENERIC);
                    handleError("Пропущена запятая ',' ", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));

                }
                else
                {
                    if (GetNextToken().Type == TypeToken.COMMA)
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.COMMA);
                    }
                    else
                    {
                        setState(State.TVALUE);
                        handleError("Ожидалась запятая ','", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateTVALUE()
        {
            if (type.Contains(Token.Value))
            {
                setState(State.CLOSE_GENERIC);
            }
            else
            {
                if (Token.Value == ">" && GetNextToken().Type == TypeToken.ID)
                {
                    setState(State.ID);
                    handleError("Пропущен тип данных", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (type.Contains(GetNextToken().Value))
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.TVALUE);
                    }
                    else
                    {
                        setState(State.CLOSE_GENERIC);
                        handleError("Ожидался тип данных", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateCLOSE_GENERIC()
        {
            if (Token.Value == ">")
            {
                setState(State.ID);
            }
            else
            {
                if (Token.Type == TypeToken.ID && GetNextToken().Type == TypeToken.OPERATOR_ASSIGNMENT)
                {
                    setState(State.IDREM);
                    handleError("Пропущен оператор сравнения '>'", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (GetNextToken().Value == ">")
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.CLOSE_GENERIC);
                    }
                    else
                    {
                        setState(State.ID);
                        handleError("Ожидался оператор сравнения '>'", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateID()
        {
            if (Token.Type == TypeToken.ID)
            {
                setState(State.IDREM);
            }
            else
            {
                if (Token.Type == TypeToken.OPERATOR_ASSIGNMENT && GetNextToken().Value == "new")
                {
                    setState(State.NEW);
                    handleError("Пропущен идентификатор", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (GetNextToken().Type == TypeToken.ID)
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.ID);
                    }
                    else
                    {
                        setState(State.IDREM);
                        handleError("Ожидался идентификатор", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateIDREM()
        {
            if (Token.Type == TypeToken.OPERATOR_ASSIGNMENT)
            {
                setState(State.NEW);
            }
            else
            {
                if (Token.Value == "new" && GetNextToken().Type == TypeToken.DELIMETER)
                {
                    setState(State.NEW);
                    handleError("Пропущен оператор присваивания '='", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (GetNextToken().Type == TypeToken.OPERATOR_ASSIGNMENT)
                    {
                        setState(State.IDREM);
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                    }
                    else
                    {
                        setState(State.NEW);
                        handleError("Ожидался оператор присваивания '='", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateNEW()
        {
            if (Token.Value == "new")
            {
                setState(State.SPASE);
            }
            else
            {
                if (Token.Type == TypeToken.DELIMETER && GetNextToken().Value == "Dictionary")
                {
                    setState(State.DICT_CREATTION);
                    handleError("Пропущено ключевое слово 'new'", "",
                       (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (GetNextToken().Value == "new")
                    {
                        setState(State.NEW);
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                    }
                    else
                    {
                        setState(State.SPASE);
                        handleError("Ожидалось ключевое слово 'new'", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateSPASE()
        {
            if (Token.Type == TypeToken.DELIMETER)
            {
                setState(State.DICT_CREATTION);
            }
            else
            {
                if (Token.Value == "Dictionary" && GetNextToken().Value == "<")
                {
                    setState(State.GENERIC_TYPE2);
                    handleError("Пропущен пробел после ключевого слова 'new'", "",
                       (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (GetNextToken().Type == TypeToken.DELIMETER)
                    {
                        setState(State.SPASE);
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                    }
                    else
                    {
                        setState(State.DICT_CREATTION);
                        handleError("Ожидался пробел", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateDICT_CREATTION()
        {
            if (Token.Value == "Dictionary")
            {
                setState(State.GENERIC_TYPE2);
            }
            else
            {
                if (Token.Value == "<" && type.Contains(GetNextToken().Value))
                {
                    setState(State.TKEY2);
                    handleError("Пропущено ключевое слово 'Dictionary'", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (GetNextToken().Value == "Dictionary")
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.DICT_CREATTION);
                    }
                    else
                    {
                        setState(State.GENERIC_TYPE2);
                        handleError("Ожидалось ключевое слово 'Dictionary'", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateGENERIC_TYPE2()
        {
            if (Token.Value == "<")
            {
                setState(State.TKEY2);
            }
            else
            {
                if (type.Contains(Token.Value) && GetNextToken().Type == TypeToken.COMMA)
                {
                    setState(State.COMMA2);
                    handleError("Пропущен оператор сравнения '<'", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {

                    if (GetNextToken().Value == "<")
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.GENERIC_TYPE2);
                    }
                    else
                    {
                        handleError("Ожидался оператор сравнения '<'", Token.Value, Token.Position);
                        setState(State.TKEY2);
                    }
                }
            }
        }
        private void stateTKEY2()
        {
            if (type.Contains(Token.Value))
            {
                setState(State.COMMA2);
            }
            else
            {
                if (Token.Type == TypeToken.COMMA && type.Contains(GetNextToken().Value))
                {
                    setState(State.TVALUE2);
                    handleError("Пропущен тип данных ", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));

                }
                else
                {
                    if (type.Contains(GetNextToken().Value))
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.TKEY2);
                    }
                    else
                    {
                        handleError("Ожидался тип данных", Token.Value, Token.Position);
                        setState(State.COMMA2);

                    }
                }
            }
        }
        private void stateCOMMA2()
        {
            if (Token.Type == TypeToken.COMMA)
            {
                setState(State.TVALUE2);
            }
            else
            {
                if (type.Contains(Token.Value) && GetNextToken().Value == ">")
                {
                    setState(State.CLOSE_GENERIC2);
                    handleError("Пропущена запятая ',' ", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));

                }
                else
                {
                    if (GetNextToken().Type == TypeToken.COMMA)
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.COMMA2);
                    }
                    else
                    {
                        setState(State.TVALUE2);
                        handleError("Ожидалась запятая ','", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateTVALUE2()
        {
            if (type.Contains(Token.Value))
            {
                setState(State.CLOSE_GENERIC2);
            }
            else
            {
                if (Token.Value == ">" && GetNextToken().Value == "(")
                {
                    setState(State.OPEN_PAREN);
                    handleError("Пропущен тип данных", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (type.Contains(GetNextToken().Value))
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.TVALUE2);
                    }
                    else
                    {
                        setState(State.CLOSE_GENERIC2);
                        handleError("Ожидался тип данных", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateCLOSE_GENERIC2()
        {
            if (Token.Value == ">")
            {
                setState(State.OPEN_PAREN);
            }
            else
            {
                if (Token.Value == "(" && GetNextToken().Value == ")")
                {
                    setState(State.CLOSE_PAREN);
                    handleError("Пропущен оператор сравнения '>'", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (GetNextToken().Value == ">")
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.CLOSE_GENERIC2);
                    }
                    else
                    {
                        setState(State.OPEN_PAREN);
                        handleError("Ожидался оператор сравнения '>'", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateOPEN_PAREN()
        {
            if (Token.Value == "(")
            {
                setState(State.CLOSE_PAREN);
            }
            else
            {
                if (Token.Value == ")" && GetNextToken().Type == TypeToken.OPERATOR_END)
                {
                    setState(State.END);
                    handleError("Пропущена круглая скобка '('", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (GetNextToken().Value == "(")
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.OPEN_PAREN);
                    }
                    else
                    {
                        setState(State.CLOSE_PAREN);
                        handleError("Ожидалась круглая скобка '('", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateCLOSE_PAREN()
        {
            if (Token.Value == ")")
            {
                setState(State.END);
            }
            else
            {
                if (Token.Type == TypeToken.OPERATOR_END)
                {
                    setState(State.START);
                    handleError("Пропущена круглая скобка ')'", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (GetNextToken().Value == ")")
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.CLOSE_PAREN);
                    }
                    else
                    {
                        setState(State.END);
                        handleError("Ожидалась круглая скобка ')'", Token.Value, Token.Position);
                    }
                }

            }
        }
        private void stateEND()
        {
            if (Token.Type == TypeToken.OPERATOR_END)
            {
                setState(State.START);
            }
            else
            {
                if (Token.Value == "Dictionary")
                {
                    setState(State.START);
                    handleError("Пропущен конец оператора ';", "",
                        (Token.Position.Item1 == 0 ? (Token.Position.Item1, Token.Position.Item1) : (Token.Position.Item1 - 1, Token.Position.Item1 - 1)));
                }
                else
                {
                    if (GetNextToken().Type == TypeToken.OPERATOR_END)
                    {
                        handleError("Ошибочный фрагмент", Token.Value, Token.Position);
                        setState(State.END);
                    }
                    else
                    {
                        setState(State.START);
                        handleError("Ожидался конец оператора ';'", Token.Value, Token.Position);
                    }
                }

            }
        }


    }
}

