using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace FrankCompiler.Test
{
    [TestClass]
    public class ScannerTest
    {
        public const string goodLine1 = "~#X ( s(X) & n(X) & c(X) )";
        public const string goodLine2 = "@X ( ~s(X) | ~(X) | ~c(X) )";
        public const string goodLine3 = "Hola mundo";
        public const string badLine2 = "~ _hola";
        public const string badLine1 = "2 + 5";

        [TestMethod]
        public void ScanerTest1()
        {
            FrankCompiler.Core.Scaner scanner = new Core.Scaner();
            var result = scanner.Scan(goodLine1);
            foreach (var item in result.Tokens)
            {
                System.Diagnostics.Trace.WriteLine(item);

            }
            Assert.AreEqual(19, result.Tokens.Count);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void ScanerTestMultiLine()
        {
            FrankCompiler.Core.Scaner scanner = new Core.Scaner();
            string[] input = {
                               goodLine1,
                               goodLine2,
                             };
            var result = scanner.Scan(input);
            foreach (var item in result.Tokens)
            {
                System.Diagnostics.Trace.WriteLine(item);

            }
            Assert.AreEqual(39, result.Tokens.Count);
            Assert.AreEqual(0, result.Errors.Count);
            
        }

        [TestMethod]
        public void TestScanerError()
        {
            FrankCompiler.Core.Scaner scanner = new Core.Scaner();
            string[] input = {
                               goodLine1,
                               goodLine2,
                               goodLine3,
                               badLine2,
                               badLine1
                             };
            var result = scanner.Scan(input);
            foreach (var item in result.Tokens)
            {
                System.Diagnostics.Trace.WriteLine(item);

            }
            Assert.AreEqual(42, result.Tokens.Count);
            Assert.AreEqual(2, result.Errors.Count);

        }


        [TestMethod]
        public void ScanerTestScanFromFile()
        {
            var filename = "";
            var scanner = new Core.Scaner();
            var result = scanner.ScanFromFile(filename);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(0, result.Tokens.Count);

            filename = @"TestCases\Scanner\Code.testcase1.txt";
            result = scanner.ScanFromFile(filename);
            foreach (var err in result.Errors)
            {
                Trace.WriteLine(err);
            }
            Assert.AreEqual(19, result.Tokens.Count);
            Assert.IsTrue(result.Errors.Count == 0);
            foreach (var item in result.Tokens)
            {
                Trace.WriteLine(item);
            }
        }



       
    }
}
