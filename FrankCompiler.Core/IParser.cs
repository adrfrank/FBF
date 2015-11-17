using System.Collections.Generic;

namespace FrankCompiler.Core
{
    public interface IParser
    {
        ParserResult Result { get; set; }       
        List<Token> Tokens { get; set; }
        ScanResult ScanerResult { get; set; }
        IScaner Scaner { get; set; }
        ParserResult Parse();
        ParserResult Parse(string line);
        ParserResult Parse(string[] lines);
        ParserResult ParseFromFile(string filepath);

       
    }
}