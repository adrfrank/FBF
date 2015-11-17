using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrankCompiler.Core
{
    public class Compiler : ICompiler
    {
        IParser parser;
        IScaner scaner;

        public CompilerResult Compile(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("El texto no puede ser vacio o nulo");
            throw new NotImplementedException();
        }

        public virtual CompilerResult CompileFromFile(string path){
            if (!System.IO.File.Exists(path))
                throw new ArgumentException("El archivo " + path + " no existe", "path");
            parser = new FormulasBienFormadasParser();
            parser.Scaner = new Scaner();
            var presult = parser.ParseFromFile(path);

            var cresult = new CompilerResult(parser.ScanerResult,presult);
            var outfile = path.Replace(Path.GetExtension(path),".eje");
            File.WriteAllLines(outfile, cresult.GeneratedCode);
            cresult.OutFile = outfile;
            //System.Diagnostics.Process.Start(outfile);
            return cresult;
        }
    }
}
