using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrankCompiler.Core
{
    public class CompilerResult
    {
        ScanResult scanResult;
        ParserResult parseResult;
        public bool ParseOK { get { return parseResult.Errors.Count == 0; } }

        public bool ScanOK { get { return scanResult.Errors.Count == 0; } }

        public bool CompilationOK { get { return ParseOK && ScanOK; } }

        public List<CompilerError> Errors { get {
            return scanResult.Errors.Union(parseResult.Errors).ToList();
        } }

        public List<string> GeneratedCode { get { return parseResult.GeneratedProgram; } }

        public List<Token> Tokens { get { return scanResult.Tokens; } }

        public string OutFile { get; set; }


        public CompilerResult(ScanResult scanresult, ParserResult parseresult) {
            scanResult = scanresult;
            parseResult = parseresult;
        }
    }
}
