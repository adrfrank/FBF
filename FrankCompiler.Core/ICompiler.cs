namespace FrankCompiler.Core
{
    public interface ICompiler
    {
        CompilerResult CompileFromFile(string path);

        CompilerResult Compile(string text);
    }
}