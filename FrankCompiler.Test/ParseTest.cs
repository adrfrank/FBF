using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrankCompiler.Core;
using System.Diagnostics;

namespace FrankCompiler.Test
{
    [TestClass]
    public class ParseTest
    {
        [TestMethod]
        public void VariasPruebas()
        {
            ParseTestFunc("@X m(X)");
            ParseTestFunc("@X Y(X)", 1);
            ParseTestFunc("~#X ( s(X) & n(X) & c(X) )");
            ParseTestFunc("@X( ~( s(X) & n(X) ) | ~c(X) ) = @X(s(X)&n(X)->~c(X))");
            ParseTestFunc("", 1);
            ParseTestFunc("~#X ( X )"); // se defasan los errores
            ParseTestFunc("@X(s(X)&n(X)->g(X))");
            ParseTestFunc("@X(g(X)->~c(X))");
            ParseTestFunc("#X~(~g(X)|~c(X))");
            ParseTestFunc("#X(g(X)&c(X))");
            ParseTestFunc("#X(s(X)&n(X))");
            ParseTestFunc("~s(X)|~n(X)|~c(X)");
            ParseTestFunc("~s(X)|~n(X)|g(X)");
            ParseTestFunc("g(sk1)");
            ParseTestFunc("c(sk1)");
            ParseTestFunc("s(sk1)");
            ParseTestFunc("# X ( ~x(C)",1);
            ParseTestFunc("3", 1);
            ParseTestFunc("@X#Y n(X,Y)");
            ParseTestFunc("@X#Y ", 1);
            ParseTestFunc("X");
            ParseTestFunc("X | Y -> Z");
            ParseTestFunc("X -> Z");
            ParseTestFunc("X  Z",1);
            ParseTestFunc("P & ~Q -> R");


        }

        void ParseTestFunc(string text, int Errors=0)
        {
            Trace.WriteLine("--------------------------------");
            Trace.WriteLine(text);
            Trace.WriteLine("Errores esperados: " + Errors);
            IParser fbfParser = new FormulasBienFormadasParser();
            var result = fbfParser.Parse(text);
            Trace.WriteLine("Errores: "+result.Errors.Count);
            if (result.Errors.Count > 0)
            {               
                foreach (var e in result.Errors)
                {
                    Trace.WriteLine(e);
                }
            }
            Assert.IsNotNull(result);
            Assert.AreEqual(Errors, result.Errors.Count);
        }
    }
}
