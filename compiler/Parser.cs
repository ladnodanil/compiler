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
            ONE,
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX,
            SEVEN,
            EIGHT
        }
        private State state = State.ONE;

        private string text;

        private int pos = 0;

        private char Char = ' ';

        private string value = "";

        private int startPos;
        private int endPos;

        public List<(string, int, int)> values = new List<(string, int, int)>();  
        public Parser(string text)
        {
            this.text = text;
        }

        public void Parse()
        {
            bool endFound = false;
            while(!endFound)
            {
                Char = nextChar();
                switch (state)
                {
                    case State.ONE:
                        stateONE();
                        break;
                    case State.TWO:
                        stateTWO();
                        break;
                    case State.THREE:
                        stateTHREE();
                        break;
                    case State.FOUR:
                        stateFOUR();
                        break;
                    case State.FIVE:
                        stateFIVE();
                        break;
                    case State.SIX:
                        stateSIX();
                        break;
                    case State.SEVEN:
                        stateSEVEN();
                        break;
                    case State.EIGHT:
                        stateEIGHT();
                        break;
                    default:
                        break;
                }

                if (Char == '\0')
                {
                    endFound = true;
                }
            }
        }

        private char nextChar()
        {
            return pos < text.Length ? text[pos] : '\0';
        }

        private void setState(State nextState)
        {
            state = nextState;
            pos++;
        }

        private bool IsSymbol(char Char)
        {
            if ((Char >= 'А' && Char <= 'Я') || (Char >= 'а' && Char <= 'я') || (Char >= '0' && Char <= '9') || 
                ("#?!|/@\\$%^&*-_.".Contains(Char)))
            {
                return true;
            }
            return false;
        }

        private void stateONE()
        {
            if (IsSymbol(Char))
            {
                value += Char;
                startPos = pos;
                setState(State.TWO);
            }
            else
            {
                setState(State.ONE);
                value = "";
            }
        }

        private void stateTWO()
        {
            if (IsSymbol(Char))
            {
                value += Char;
                setState(State.THREE);
            }
            else
            {
                setState(State.ONE);
                value = "";
            }
        }

        private void stateTHREE()
        {
            if (IsSymbol(Char))
            {
                value += Char;
                setState(State.FOUR);
            }
            else
            {
                setState(State.ONE);
                value = "";
            }
        }

        private void stateFOUR()
        {
            if (IsSymbol(Char))
            {
                value += Char;
                setState(State.FIVE);
            }
            else
            {
                setState(State.ONE);
                value = "";
            }
        }

        private void stateFIVE()
        {
            if (IsSymbol(Char))
            {
                value += Char;
                setState(State.SIX);
            }
            else
            {
                setState(State.ONE);
                value = "";
            }
        }

        private void stateSIX()
        {
            if (IsSymbol(Char))
            {
                value += Char;
                setState(State.SEVEN);
            }
            else
            {
                setState(State.ONE);
                value = "";
            }
        }

        private void stateSEVEN()
        {
            if (IsSymbol(Char))
            {   
                value += Char;
                setState(State.EIGHT);
            }
            else
            {
                setState(State.ONE);
                value = "";
            }
        }

        private void stateEIGHT()
        {
            if (IsSymbol(Char))
            {
                value += Char;
                setState(State.EIGHT);
            }
            else
            {
                endPos = pos - 1;
                values.Add((value, startPos, endPos));
                setState(State.ONE);
                value = "";
            }
        }


    }
}
