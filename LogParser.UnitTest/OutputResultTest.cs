using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace LogParser.UnitTest
{
    [TestClass]
    public class OutputResultTest
    {   
        [TestMethod]
        public void PrintToFile_Scenario_Successful()
        {
            //Arrange
            List<KeyValuePair<string, int>> sortedIPList = new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> sortedURLList = new List<KeyValuePair<string, int>>();
            string outputFolder = new string("");
            string fullPath = new string("");
            setVar(ref sortedIPList, ref sortedURLList, ref outputFolder, ref fullPath);
            //Act
            OutputResults.PrintToFile(sortedIPList,sortedURLList,outputFolder,append: false);
            //Assert
            Assert.IsTrue(File.Exists(fullPath));
            using (var sr = new StreamReader(fullPath))
            {
                string output = sr.ReadToEnd();
                Assert.IsTrue(output.Contains(string.Format("Number of unique IP Addresses:{0}",3)));
                Assert.IsTrue(!output.Contains(string.Format("Number of unique URLs:{0}", 3)));
                Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "192.168.1.1","100")));
                Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "192.168.1.10", "50")));
                Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "192.168.1.100", "25")));
                Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/test/test1","100")));
                Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/test/test2","50")));
                Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/test/test3","25")));
            }
        }
        [TestMethod]
        public void PrintToFile_Scenario_SuccessfulPrintLessTopHits()
        {
            //Arrange
            List<KeyValuePair<string, int>> sortedIPList = new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> sortedURLList = new List<KeyValuePair<string, int>>();
            string outputFolder = new string("");
            string fullPath = new string("");
            setVar(ref sortedIPList, ref sortedURLList, ref outputFolder, ref fullPath);
            //Act
            OutputResults.PrintToFile(sortedIPList, sortedURLList, outputFolder, append: false);
            //Act
            OutputResults.PrintToFile(sortedIPList, sortedURLList, outputFolder, numberOfResults: 1,append: false);
            //Assert
            Assert.IsTrue(File.Exists(fullPath));
            using (var sr = new StreamReader(fullPath))
            {
                string output = sr.ReadToEnd();
                Assert.IsTrue(output.Contains(string.Format("Number of unique IP Addresses:{0}", 3)));
                Assert.IsTrue(!output.Contains(string.Format("Number of unique URLs:{0}", 3)));
                Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "192.168.1.1", "100")));
                Assert.IsTrue(!output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "192.168.1.10", "50")));
                Assert.IsTrue(!output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "192.168.1.100", "25")));
                Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/test/test1", "100")));
                Assert.IsTrue(!output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/test/test2", "50")));
                Assert.IsTrue(!output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/test/test3", "25")));
            }
        }

        [TestMethod]
        public void PrintToFile_Scenario_SuccessfulPrintMinAvailable()
        {
            //Arrange
            List<KeyValuePair<string, int>> sortedIPList = new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> sortedURLList = new List<KeyValuePair<string, int>>();
            string outputFolder = new string("");
            string fullPath = new string("");
            setVar(ref sortedIPList, ref sortedURLList, ref outputFolder, ref fullPath);
            //Act
            OutputResults.PrintToFile(sortedIPList, sortedURLList, outputFolder, append: false);
            //Act
            OutputResults.PrintToFile(sortedIPList, sortedURLList, outputFolder, numberOfResults: 10, append: false);
            //Assert
            Assert.IsTrue(File.Exists(fullPath));
            using (var sr = new StreamReader(fullPath))
            {
                string output = sr.ReadToEnd();
                Assert.IsTrue(output.Contains(string.Format("Number of unique IP Addresses:{0}", 3)));
                Assert.IsTrue(!output.Contains(string.Format("Number of unique URLs:{0}", 3)));
                Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "192.168.1.1", "100")));
                Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "192.168.1.10", "50")));
                Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "192.168.1.100", "25")));
                Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/test/test1", "100")));
                Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/test/test2", "50")));
                Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/test/test3", "25")));
            }
        }

        [TestMethod]
        public void PrintToFile_Scenario_FileNotFound()
        {
            //Arrange
            List<KeyValuePair<string, int>> sortedIPList = new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> sortedURLList = new List<KeyValuePair<string, int>>();
            string outputFolder = new string("");
            string fullPath = new string("");
            setVar(ref sortedIPList, ref sortedURLList, ref outputFolder, ref fullPath);
            //Act
            OutputResults.PrintToFile(sortedIPList, sortedURLList, outputFolder, append: false);
            outputFolder = @"..\..\..\Outiles";
            fullPath = outputFolder + @"\Output.txt";
            //Assert
            Assert.ThrowsException<DirectoryNotFoundException>(() => { OutputResults.PrintToFile(sortedIPList, sortedURLList, outputFolder, numberOfResults: 10, append: false); });
            
        }
        
        private void setVar(ref List<KeyValuePair<string, int>> sortedIPList, ref List<KeyValuePair<string, int>> sortedURLList, ref string outputFolder, ref string fullPath)
        {
            sortedIPList = new List<KeyValuePair<string, int>>();
            sortedIPList.Add(new KeyValuePair<string, int>("192.168.1.1", 100));
            sortedIPList.Add(new KeyValuePair<string, int>("192.168.1.10", 50));
            sortedIPList.Add(new KeyValuePair<string, int>("192.168.1.100", 25));
            sortedURLList = new List<KeyValuePair<string, int>>();
            sortedURLList.Add(new KeyValuePair<string, int>("/test/test1", 100));
            sortedURLList.Add(new KeyValuePair<string, int>("/test/test2", 50));
            sortedURLList.Add(new KeyValuePair<string, int>("/test/test3", 25));
            outputFolder = @"..\..\..\OutputFiles";
            fullPath = outputFolder + @"\Output.txt";
        }
    }
}
