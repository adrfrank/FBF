namespace FrankCompiler.Core
{
    public interface IScaner
    {
        ScanResult Scan(string[] input);
        ScanResult Scan(string input);
        ScanResult ScanFromFile(string filename);
    }
}