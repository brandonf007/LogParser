using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LogParser.UnitTest
{
    [TestClass]
    public class ProcessLogFileTest
    {        
        [TestMethod]
        public void ReadFromFileAndStore_Scenario_ValidRegex()
        {
            // Arrange
            string filePath = @"..\..\..\TestFiles\ISLogFile.log";
            string regex = @"(^(?:[0-9]{1,3}\.){3}[0-9]{1,3})(?:.+)(?:""{1}(?:GET|HEAD|POST|PUT|DELETE|CONNECT|OPTIONS|TRACE){1}\s{1}(.+)\s{1}(?:HTTP/1.1|HTTP/1.0|HTTP/2|HTTP/3){1}""{1})(?:.+$)";
            ProcessedLogs pLog = new ProcessedLogs();
            var pLogFile = new ProcessLogFile(filePath,regex,pLog);

            // Act
            pLogFile.ReadFromFileAndStore();

            // Assert            
            Assert.AreEqual(10000, pLogFile.successful);
            Assert.AreEqual(0,pLogFile.failed);
            Assert.IsTrue(pLog.GetIpAddresses().Count > 0);
            Assert.IsTrue(pLog.GetUrls().Count > 0);
        }

        [TestMethod]
        public void ReadFromFileAndStore_Scenario_IncorrectRegexGroupReturn()
        {
            // Arrange
            string filePath = @"..\..\..\TestFiles\ISLogFile.log";
            string regex = @".";
            ProcessedLogs pLog = new ProcessedLogs();
            var pLogFile = new ProcessLogFile(filePath, regex, pLog);

            // Assert
            Assert.ThrowsException<Exception>(() => pLogFile.ReadFromFileAndStore());
        }

        [TestMethod]
        public void ReadFromFileAndStore_Scenario_ImperfectRegex()
        {
            // Arrange
            string filePath = @"..\..\..\TestFiles\ISLogFile.log";
            string regex = @"(^(?:[0-9]{1,3}\.){3}[0-9]{1,3})(?:.+)(?:""{1}(?:GET|HEAD|POST|PUT|DELETE|CONNECT|OPTIONS|TRACE){1}\s{1}(.+)\s{1}(?:HTTP/1.1|HTTP/2|HTTP/3){1}""{1})(?:.+$)";
            ProcessedLogs pLog = new ProcessedLogs();
            var pLogFile = new ProcessLogFile(filePath, regex, pLog);

            // Act
            pLogFile.ReadFromFileAndStore();

            // Assert            
            Assert.IsTrue(pLogFile.successful > 0);
            Assert.IsTrue(pLogFile.failed > 0);
            Assert.IsTrue(pLog.GetIpAddresses().Count > 0);
            Assert.IsTrue(pLog.GetUrls().Count > 0);
        }

        [TestMethod]
        public void ReadFromFileAndStore_Scenario_FailedRegex()
        {
            // Arrange
            string filePath = @"..\..\..\TestFiles\ISLogFile.log";
            string regex = @"(Rat)(Bat)";
            ProcessedLogs pLog = new ProcessedLogs();
            var pLogFile = new ProcessLogFile(filePath, regex, pLog);

            // Act
            pLogFile.ReadFromFileAndStore();

            // Assert            
            Assert.IsTrue(pLogFile.successful == 0);
            Assert.IsTrue(pLogFile.failed > 0);
            Assert.IsTrue(pLog.GetIpAddresses().Count == 0);
            Assert.IsTrue(pLog.GetUrls().Count == 0);
        }
    }
}
