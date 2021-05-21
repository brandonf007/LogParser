using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogParser.UnitTest
{
    [TestClass]
    public class SortProcessLogsTest
    {
        [TestMethod]
        public void BeginSort_Scenario_SortAscending()
        {
            // Arrange
            ProcessedLogs pLogs = new ProcessedLogs();
            for (int i = 0; i < 10; i++)
                pLogs.AddIpAddress("192.168.1.0");
            for (int i = 0; i < 9; i++)
                pLogs.AddIpAddress("192.168.1.1");
            for (int i = 0; i < 8; i++)
                pLogs.AddIpAddress("192.168.1.2");

            for (int i = 0; i < 10; i++)
                pLogs.AddURL("/Test/10");
            for (int i = 0; i < 9; i++)
                pLogs.AddURL("/Test/9");
            for (int i = 0; i < 8; i++)
                pLogs.AddURL("/Test/8");

            SortProcessedLogs sLogs = new SortProcessedLogs(pLogs);

            // Act
            sLogs.BeginSort(true);

            //Assert
            Assert.IsTrue(sLogs.sortedIPList.ToArray()[0].Key == "192.168.1.2" && sLogs.sortedIPList.ToArray()[0].Value == 8);
            Assert.IsTrue(sLogs.sortedIPList.ToArray()[1].Key == "192.168.1.1" && sLogs.sortedIPList.ToArray()[1].Value == 9);
            Assert.IsTrue(sLogs.sortedIPList.ToArray()[2].Key == "192.168.1.0" && sLogs.sortedIPList.ToArray()[2].Value == 10);
            Assert.IsTrue(sLogs.sortedUrlList.ToArray()[0].Key == "/Test/8" && sLogs.sortedUrlList.ToArray()[0].Value == 8);
            Assert.IsTrue(sLogs.sortedUrlList.ToArray()[1].Key == "/Test/9" && sLogs.sortedUrlList.ToArray()[1].Value == 9);
            Assert.IsTrue(sLogs.sortedUrlList.ToArray()[2].Key == "/Test/10" && sLogs.sortedUrlList.ToArray()[2].Value == 10);
            
        }

        [TestMethod]
        public void BeginSort_Scenario_SortDescending()
        {
            // Arrange
            ProcessedLogs pLogs = new ProcessedLogs();
            for (int i = 0; i < 8; i++)
                pLogs.AddIpAddress("192.168.1.2");
            for (int i = 0; i < 9; i++)
                pLogs.AddIpAddress("192.168.1.1");            
            for (int i = 0; i < 10; i++)
                pLogs.AddIpAddress("192.168.1.0");

            for (int i = 0; i < 8; i++)
                pLogs.AddURL("/Test/8");
            for (int i = 0; i < 9; i++)
                pLogs.AddURL("/Test/9");            
            for (int i = 0; i < 10; i++)
                pLogs.AddURL("/Test/10");

            SortProcessedLogs sLogs = new SortProcessedLogs(pLogs);

            // Act
            sLogs.BeginSort();

            //Assert
            Assert.IsTrue(sLogs.sortedIPList.ToArray()[2].Key == "192.168.1.2" && sLogs.sortedIPList.ToArray()[2].Value == 8);
            Assert.IsTrue(sLogs.sortedIPList.ToArray()[1].Key == "192.168.1.1" && sLogs.sortedIPList.ToArray()[1].Value == 9);
            Assert.IsTrue(sLogs.sortedIPList.ToArray()[0].Key == "192.168.1.0" && sLogs.sortedIPList.ToArray()[0].Value == 10);
            Assert.IsTrue(sLogs.sortedUrlList.ToArray()[2].Key == "/Test/8" && sLogs.sortedUrlList.ToArray()[2].Value == 8);
            Assert.IsTrue(sLogs.sortedUrlList.ToArray()[1].Key == "/Test/9" && sLogs.sortedUrlList.ToArray()[1].Value == 9);
            Assert.IsTrue(sLogs.sortedUrlList.ToArray()[0].Key == "/Test/10" && sLogs.sortedUrlList.ToArray()[0].Value == 10);

        }
    }
}
