using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace compiler
{
    public class Lexerr
    {
        public List<Token> Tokens = new List<Token>();

        private char currentChar;

        private string codeText;


        public Lexerr(string code)
        {
            codeText = code.Replace("\n", " ").Replace("\t", " ").Replace("\r", " ");
        }

        public void Lex()
        {

        }
    }
}
