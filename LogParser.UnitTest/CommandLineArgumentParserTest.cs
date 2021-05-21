using CommandLineParser.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LogParser.UnitTest
{
    [TestClass]
    public class CommandLineArgumentParserTest
    {
        [TestMethod]
        public void ParseCommandLineArguments_Scenario_ValidArgs()
        {
            //Arrange
            string[] args = new string[] { "-f", @"..\..\..\TestFiles\ISLogFile.log" };
            string outputPath = "";
            string regexPattern = "";
            string file = "";
            //Act
            CommandLineArgumentParser.ParseCommandLineArguments(args,out outputPath,out regexPattern,out file);
            //Assert
            Assert.IsNull(outputPath);
            Assert.AreEqual(@"(^(?:[0-9]{1,3}\.){3}[0-9]{1,3})(?:.+)(?:""{1}(?:GET|HEAD|POST|PUT|DELETE|CONNECT|OPTIONS|TRACE){1}\s{1}(.+)\s{1}(?:HTTP/1.1|HTTP/1.0|HTTP/2|HTTP/3){1}""{1})(?:.+$)", regexPattern);
            Assert.AreEqual(@"..\..\..\TestFiles\ISLogFile.log", file);
        }
        [TestMethod]
        public void ParseCommandLineArguments_Scenario_NoArgPassed()
        {
            //Arrange
            string[] args = { };
            string outputPath = "";
            string regexPattern = "";
            string file = "";
            //Assert
            Assert.ThrowsException<MandatoryArgumentNotSetException>(() => CommandLineArgumentParser.ParseCommandLineArguments(args, out outputPath, out regexPattern, out file), "Argument f(filePath) is not marked as optional and was not found on the command line.");


        }
        [TestMethod]
        public void ParseCommandLineArguments_Scenario_InvalidFileArgPassed()
        {
            //Arrange            
            string[] args = new string[] { "-f" };
            string outputPath = "";
            string regexPattern = "";
            string file = "";
            //Assert
            Assert.ThrowsException<CommandLineArgumentException>(() => CommandLineArgumentParser.ParseCommandLineArguments(args, out outputPath, out regexPattern, out file), "Value argument f(filePath) must be followed by a value.");

        }
        [TestMethod]
        public void ParseCommandLineArguments_Scenario_InvalidRegexPassed()
        {
            //Arrange
            string[] args = new string[] { "-f", @"..\..\..\TestFiles\ISLogFile.log", "-r", "[" };
            string outputPath = "";
            string regexPattern = "";
            string file = "";
            try
            {
                //Act
                CommandLineArgumentParser.ParseCommandLineArguments(args, out outputPath, out regexPattern, out file);
            }
            catch (Exception ex)
            {
                //Assert
                // RegexParseException thrown
                Assert.IsTrue(ex.Message == "Invalid pattern '[' at offset 1. Unterminated [] set.");
            }
            //Assert.ThrowsException<RegexParseException>(() => CommandLineArgumentParser.ParseCommandLineArguments(args, out outputPath, out regexPattern, out file), "Provided Regular Expression pattern can not be empty, it should contain two groups the first identifying the IP Address, and the second identifying the URL");
        }
        [TestMethod]
        public void ParseCommandLineArguments_Scenario_FilePathFileDoesNotExist()
        {
            //Arrange
            string[] args = new string[] { "-f", @"..\..\..\TestFiles\DoesNotExist.log", "-r", "" };
            string outputPath = "";
            string regexPattern = "";
            string file = "";
            //Assert
            Assert.ThrowsException<Exception>(() => CommandLineArgumentParser.ParseCommandLineArguments(args, out outputPath, out regexPattern, out file), "The file provided does not exist: ..\\..\\..\\TestFiles\\DoesNotExist.log");
        }
    }
}
