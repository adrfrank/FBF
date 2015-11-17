using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrankCompiler.Core
{
    public abstract class AbstractParser : IParser
    {
        List<Token> tokens;
        ScanResult scanResult;
        ParserResult parserResult;
        IScaner scaner;
        protected int idx;
        protected Token t;
        public ParserResult Result
        {
            get
            {
                return parserResult;
            }

            set
            {
                parserResult = value;
            }
        }

        public List<Token> Tokens
        {
            get
            {
                return tokens;
            }

            set
            {
                tokens = value;
            }
        }

        public ScanResult ScanerResult
        {
            get
            {
                return scanResult;
            }
            set
            {
                scanResult = value;
                InitParse();
            }
        }

        public IScaner Scaner
        {
            get
            {
                return scaner;
            }

            set
            {
                scaner = value;
            }
        }

        void InitParse()
        {
            Result = new ParserResult();
            Tokens = ScanerResult.Tokens;
            idx = 0;
        }

        protected abstract void EntryPoint();

        public ParserResult Parse()
        {
            if (ScanerResult == null)
                AddError("No hay resultados de parser");
            if (Tokens.Count == 0)
                AddError("No hay tokens");
            if (ScanerResult.Errors.Count > 0)
                foreach (var e in ScanerResult.Errors)
                    Result.Errors.Add(e);
            if (Result.Errors.Count == 0)
                EntryPoint();
            return Result;
        }

        public ParserResult Parse(string line)
        {
            ScanerResult = scaner.Scan(line);
            return Parse();
        }

        public ParserResult Parse(string[] lines)
        {
            ScanerResult = scaner.Scan(lines);
            return Parse();

        }

        public ParserResult ParseFromFile(string filepath)
        {
            ScanerResult = scaner.ScanFromFile(filepath);
            return Parse();
        }

        protected Token NextToken()
        {
            if (idx >= Tokens.Count || idx < 0)
                t = new Token(TokenType.NoToken, "", 0);
            else
                t = Tokens[idx++];
            return t;
        }

        protected void AddError(string message, int line = 0, int column = 0)
        {
            var type = ErrorType.Syntax;
            Result.Errors.Add(new CompilerError() { Column = column, Line = line, Type = type, Message = message });
        }

        protected void AddError(string message, Token token)
        {
            Token tmp;
            if (idx > 0 && token.Type == TokenType.NoToken)
            {
                tmp = Tokens[idx - 1];
                AddError(message, tmp.Line, tmp.Column);
            }
            else
                AddError(message + " y llegó " + token.Lexem, token.Line, token.Column);
        }
    }
}
