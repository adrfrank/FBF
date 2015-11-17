using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrankCompiler.Core
{
    public class ParserResult
    {
        public List<CompilerError> Errors { get; set; }

        public List<string> GeneratedProgram { get; set; }

        public ParserResult()
        {
            Errors = new List<CompilerError>();
        }
    }
}
