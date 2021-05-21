using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace LogParser.UnitTest
{
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public void IntegrationTest_Scenario_ProvidedExampleFile()
        {
            string file = "";
            string outputFile = "";
            string regexPattern = "";
            string[] args = { "-f", @"..\..\..\TestFiles\ProvidedExample.log", "-o", @"..\..\..\OutputFiles" };
            try
            {
                // Only run if the -h --help flags are not passed as arguments
                if (CommandLineArgumentParser.ParseCommandLineArguments(args, out outputFile, out regexPattern, out file))
                {
                    // Assert that Command Line arguments are parsed as expected
                    Assert.AreEqual(@"..\..\..\OutputFiles", outputFile);
                    Assert.AreEqual(@"(^(?:[0-9]{1,3}\.){3}[0-9]{1,3})(?:.+)(?:""{1}(?:GET|HEAD|POST|PUT|DELETE|CONNECT|OPTIONS|TRACE){1}\s{1}(.+)\s{1}(?:HTTP/1.1|HTTP/1.0|HTTP/2|HTTP/3){1}""{1})(?:.+$)", regexPattern);
                    Assert.AreEqual(@"..\..\..\TestFiles\ProvidedExample.log", file);
                    ProcessedLogs pLogs = new ProcessedLogs();
                    // Process the log files
                    ProcessLogFile logger = new ProcessLogFile(file, regexPattern, pLogs);
                    logger.ReadFromFileAndStore();
                    // Assert - pLogs was populated as expected                    
                    Assert.AreEqual(23, logger.successful);
                    Assert.AreEqual(0,logger.failed);
                    Assert.AreEqual(11,pLogs.GetIpAddresses().Count);
                    Assert.AreEqual(22,pLogs.GetUrls().Count);

                    // Sort the processed logs
                    SortProcessedLogs sort = new SortProcessedLogs(pLogs);
                    sort.BeginSort(thenBy: true);
                    Assert.IsTrue(sort.sortedIPList.ToArray()[0].Key == "168.41.191.40" && sort.sortedIPList.ToArray()[0].Value == 4);
                    Assert.IsTrue(sort.sortedIPList.ToArray()[1].Key == "72.44.32.10" && sort.sortedIPList.ToArray()[1].Value == 3);
                    Assert.IsTrue(sort.sortedIPList.ToArray()[2].Key == "50.112.00.11" && sort.sortedIPList.ToArray()[2].Value == 3);
                    Assert.IsTrue(sort.sortedUrlList.ToArray()[0].Key == "/docs/manage-websites/" && sort.sortedUrlList.ToArray()[0].Value == 2);
                    Assert.IsTrue(sort.sortedUrlList.ToArray()[1].Key == "http://example.net/faq/" && sort.sortedUrlList.ToArray()[1].Value == 1);
                    Assert.IsTrue(sort.sortedUrlList.ToArray()[2].Key == "http://example.net/blog/category/meta/" && sort.sortedUrlList.ToArray()[2].Value == 1);

                    string fileName = "ProvidedExampleOutputFile.txt";
                    if (!string.IsNullOrEmpty(outputFile))
                        OutputResults.PrintToFile(sort.sortedIPList, sort.sortedUrlList, outputFile,fileName: fileName, append:false);

                    string fullPath = outputFile + @"\" + fileName;
                    Assert.IsTrue(File.Exists(fullPath));
                    using (var sr = new StreamReader(fullPath))
                    {
                        string output = sr.ReadToEnd();
                        Assert.IsTrue(output.Contains(string.Format("Number of unique IP Addresses:{0}", 11)));                        
                        Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "168.41.191.40", "4")));
                        Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "72.44.32.10", "3")));
                        Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "50.112.00.11", "3")));
                        Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/docs/manage-websites/", "2")));
                        Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "http://example.net/faq/", "1")));
                        Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "http://example.net/blog/category/meta/", "1")));
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void IntegrationTest_Scenario_AdditionalExampleFile()
        {
            string file = "";
            string outputFile = "";
            string regexPattern = "";
            string[] args = { "-f", @"..\..\..\TestFiles\ISLogFile.log", "-o", @"..\..\..\OutputFiles" };
            try
            {
                // Only run if the -h --help flags are not passed as arguments
                if (CommandLineArgumentParser.ParseCommandLineArguments(args, out outputFile, out regexPattern, out file))
                {
                    // Assert that Command Line arguments are parsed as expected
                    Assert.AreEqual(@"..\..\..\OutputFiles", outputFile);
                    Assert.AreEqual(@"(^(?:[0-9]{1,3}\.){3}[0-9]{1,3})(?:.+)(?:""{1}(?:GET|HEAD|POST|PUT|DELETE|CONNECT|OPTIONS|TRACE){1}\s{1}(.+)\s{1}(?:HTTP/1.1|HTTP/1.0|HTTP/2|HTTP/3){1}""{1})(?:.+$)", regexPattern);
                    Assert.AreEqual(@"..\..\..\TestFiles\ISLogFile.log", file);
                    ProcessedLogs pLogs = new ProcessedLogs();
                    // Process the log files
                    ProcessLogFile logger = new ProcessLogFile(file, regexPattern, pLogs);
                    logger.ReadFromFileAndStore();
                    // Assert - pLogs was populated as expected                    
                    Assert.AreEqual(10000, logger.successful);
                    Assert.AreEqual(0, logger.failed);
                    Assert.AreEqual(1753, pLogs.GetIpAddresses().Count);
                    Assert.AreEqual(1498, pLogs.GetUrls().Count);

                    // Sort the processed logs
                    SortProcessedLogs sort = new SortProcessedLogs(pLogs);
                    sort.BeginSort(thenBy: true);
                    Assert.IsTrue(sort.sortedIPList.ToArray()[0].Key == "66.249.73.135" && sort.sortedIPList.ToArray()[0].Value == 482);
                    Assert.IsTrue(sort.sortedIPList.ToArray()[1].Key == "46.105.14.53" && sort.sortedIPList.ToArray()[1].Value == 364);
                    Assert.IsTrue(sort.sortedIPList.ToArray()[2].Key == "130.237.218.86" && sort.sortedIPList.ToArray()[2].Value == 357);
                    Assert.IsTrue(sort.sortedUrlList.ToArray()[0].Key == "/favicon.ico" && sort.sortedUrlList.ToArray()[0].Value == 807);
                    Assert.IsTrue(sort.sortedUrlList.ToArray()[1].Key == "/style2.css" && sort.sortedUrlList.ToArray()[1].Value == 546);
                    Assert.IsTrue(sort.sortedUrlList.ToArray()[2].Key == "/reset.css" && sort.sortedUrlList.ToArray()[2].Value == 538);

                    string fileName = "AdditionalExampleOutputFile.txt";
                    if (!string.IsNullOrEmpty(outputFile))
                        OutputResults.PrintToFile(sort.sortedIPList, sort.sortedUrlList, outputFile, fileName: fileName, append: false);

                    string fullPath = outputFile + @"\" + fileName;
                    Assert.IsTrue(File.Exists(fullPath));
                    using (var sr = new StreamReader(fullPath))
                    {
                        string output = sr.ReadToEnd();
                        Assert.IsTrue(output.Contains(string.Format("Number of unique IP Addresses:{0}", 1753)));
                        Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "66.249.73.135", "482")));
                        Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "46.105.14.53", "364")));
                        Assert.IsTrue(output.Contains(string.Format("Most Active IP Addresses:{0} - HITS:{1}", "130.237.218.86", "357")));
                        Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/favicon.ico", "807")));
                        Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/style2.css", "546")));
                        Assert.IsTrue(output.Contains(string.Format("Most visited URLS:{0} - Occurences:{1}", "/reset.css", "538")));
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
