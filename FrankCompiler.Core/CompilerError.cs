using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrankCompiler.Core
{
    public enum ErrorType
    {
        Lexicographic,
        Syntax,
        Grammar
    }
    public class CompilerError
    {
        public ErrorType Type { get; set; }
        public int Line { get; set; }

        public int Column { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} - Linea {2} Columna {3}",Type,Message,Line,Column);
        }

    }
}
