using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace compiler
{

    public  class Parser
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


        public List<Token> Tokens = new List<Token>();

        public List<ParseError> errors = new List<ParseError>();

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

        private void handleError(string message, string value,(int,int) pos)
        {
            errors.Add(new ParseError(message,value,pos));
            //setState(State.ERROR);
        }
        public void Parse()
        {
            foreach(Token token in Tokens)
            {
                Token = token;
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
            }
        }
        private void setState(State value)
        {
            state = value;
        }
        private void stateSTART()
        {
            if(Token.Value == "Dictionary")
            {
                setState(State.GENERIC_TYPE);
            }
            else
            {
                handleError("Ожидалось ключевое слово 'Dictionary'",Token.Value,Token.Position);

            }
        }

        private void stateGENERIC_TYPE()
        {
            if(Token.Value == "<")
            {
                setState(State.TKEY);
            }
            else
            {
                handleError("Ожидался оператор сравнения '<'", Token.Value, Token.Position);
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
                handleError("Ожидался тип данных", Token.Value, Token.Position);
            }
        }
        private void stateCOMMA()
        {
            if(Token.Type == TypeToken.COMMA)
            {
                setState(State.TVALUE);
            }
            else
            {
                handleError("Ожидалась запятая ','", Token.Value, Token.Position);
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
                handleError("Ожидался тип данных", Token.Value, Token.Position);
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
                handleError("Ожидался оператор сравнения '>'", Token.Value, Token.Position);
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
                handleError("Ожидался идентификатор", Token.Value, Token.Position);
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
                handleError("Ожидался оператор присваивания '='", Token.Value, Token.Position);
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
                handleError("Ожидалось ключевое слово 'new'", Token.Value, Token.Position);
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
                handleError("Ожидался пробел", Token.Value, Token.Position);
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
                handleError("Ожидалось ключевое слово 'Dictionary'", Token.Value, Token.Position);
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
                handleError("Ожидался оператор сравнения '<'", Token.Value, Token.Position);
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
                handleError("Ожидался тип данных", Token.Value, Token.Position);
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
                handleError("Ожидалась запятая ','", Token.Value, Token.Position);
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
                handleError("Ожидался тип данных", Token.Value, Token.Position);
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
                handleError("Ожидался оператор сравнения '>'", Token.Value, Token.Position);
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
                handleError("Ожидалась круглая скобка '('", Token.Value, Token.Position);
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
                handleError("Ожидалась круглая скобка ')'", Token.Value, Token.Position);
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
                handleError("Ожидался конец оператора ';'", Token.Value, Token.Position );
            }
        }


    }
}
