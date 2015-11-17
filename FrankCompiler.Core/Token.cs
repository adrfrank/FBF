using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrankCompiler.Core
{
    public enum TokenType
    {
        NoToken,
        Identi,
        PalRes,
        CteEnt,
        CteDec,
        Delimi,
        CteCad,
        OpArit,
        OpAsig,
        OpRela,
        OpLog,
        CteLog,
        Cuantif
    }

    public class Token
    {
        static Token _notoken = null;
        public static Token NoToken
        {
            get
            {
                if (_notoken == null)
                    _notoken = new Token(TokenType.NoToken, "");
                return _notoken;
            }
        }
        public string Lexem { get; set; }

        public string LexemLower { get { return Lexem.ToLower(); } }
        public TokenType Type { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public Token(TokenType type, string lexem, int line = 0)
        {
            Lexem = lexem;
            Type = type;
            Line = line;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}   -line: {2} -Column: {3}", Type, Lexem, Line,Column);
        }
    }
}
