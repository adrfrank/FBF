using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrankCompiler.Core
{
    public class ScanResult
    {
        public List<Token> Tokens { get; set; }
        public List<CompilerError> Errors { get; set; }

        public ScanResult()
        {
            Tokens = new List<Token>();
            Errors = new List<CompilerError>();
        }
    }
}
