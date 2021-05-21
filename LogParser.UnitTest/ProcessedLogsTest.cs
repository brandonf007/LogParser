using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogParser.UnitTest
{
    [TestClass]
    public class ProcessedLogsTest
    {
        [TestMethod]
        public void AddIpAddress_Scenario_UniquelyAddingToIPAddressDictionary()
        {
            //Arrange
            ProcessedLogs pLogs = new ProcessedLogs();
            List<string> ipAddress = new List<string>();
            for (int i = 0; i < 255000 / 255; i++)
            {
                for (int y = 0; y <= 255; y++)
                {
                    ipAddress.Add(string.Format("192.168.{0}.{1}",i,y));
                }
            }
            //Act
            ipAddress.ForEach(x => { pLogs.AddIpAddress(x); });
            //Assert
            Assert.IsTrue(pLogs.GetIpAddresses().Count == 256000);
        }

        [TestMethod]
        public void AddIpAddress_Scenario_AddingToIPAddressDictionary()
        {
            //Arrange
            ProcessedLogs pLogs = new ProcessedLogs();
            List<string> ipAddress = new List<string>();
            for (int i = 0; i < 255000 / 255; i++)
            {
                for (int y = 0; y <= 255; y++)
                {
                    ipAddress.Add(string.Format("192.168.{0}.100", i));
                }
            }
            //Act
            ipAddress.ForEach(x => { pLogs.AddIpAddress(x); });
            //Assert
            Assert.IsTrue(pLogs.GetIpAddresses().Count == 1000);
            int numberOfClashes = 0;
            pLogs.GetIpAddresses().TryGetValue("192.168.0.100", out numberOfClashes);
            Assert.IsTrue(numberOfClashes == 256);

        }

        [TestMethod]
        public void AddURL_Scenario_UniquelyAddingToURLDictionary()
        {
            //Arrange
            ProcessedLogs pLogs = new ProcessedLogs();
            List<string> url = new List<string>();
            for (int i = 0; i < 255000 / 255; i++)
            {
                for (int y = 0; y <= 255; y++)
                {
                    url.Add(string.Format("/Test/Test{0}/{1}", i, y));
                }
            }
            //Act
            url.ForEach(x => { pLogs.AddURL(x); });
            //Assert
            Assert.IsTrue(pLogs.GetUrls().Count == 256000);
        }

        [TestMethod]
        public void AddURL_Scenario_AddingToURLDictionary()
        {
            //Arrange
            ProcessedLogs pLogs = new ProcessedLogs();
            List<string> url = new List<string>();
            for (int i = 0; i < 255000 / 255; i++)
            {
                for (int y = 0; y <= 255; y++)
                {
                    url.Add(string.Format("/Test/Test/{0}", i));
                }
            }
            //Act
            url.ForEach(x => { pLogs.AddURL(x); });
            //Assert
            Assert.IsTrue(pLogs.GetUrls().Count == 1000);
            int numberOfClashes = 0;
            pLogs.GetUrls().TryGetValue("/Test/Test/0", out numberOfClashes);
            Assert.IsTrue(numberOfClashes == 256);

        }

        [TestMethod]
        public void AddToURLAndIP_Scenario_Adding()
        {
            //Arrange
            ProcessedLogs pLogs = new ProcessedLogs();
            List<string> url = new List<string>();
            for (int i = 0; i < 255000 / 255; i++)
            {
                for (int y = 0; y <= 255; y++)
                {
                    url.Add(string.Format("/Test/Test/{0}", i));
                }
            }
            List<string> ipAddress = new List<string>();
            for (int i = 0; i < 255000 / 255; i++)
            {
                for (int y = 0; y <= 255; y++)
                {
                    ipAddress.Add(string.Format("192.168.{0}.100", i));
                }
            }
            //Act
            Parallel.ForEach(ipAddress, x => { pLogs.AddIpAddress(x); });
            Parallel.ForEach(url, x => { pLogs.AddURL(x); });
            //Assert
            Assert.IsTrue(pLogs.GetUrls().Count == 1000);
            int numberOfURLClashes = 0;
            pLogs.GetUrls().TryGetValue("/Test/Test/0", out numberOfURLClashes);
            Assert.IsTrue(numberOfURLClashes == 256);
            Assert.IsTrue(pLogs.GetIpAddresses().Count == 1000);
            int numberOfIPClashes = 0;
            pLogs.GetIpAddresses().TryGetValue("192.168.0.100", out numberOfIPClashes);
            Assert.IsTrue(numberOfIPClashes == 256);
        }

        [TestMethod]
        public void AddToURLAndIP_Scenario_AddingUniquely()
        {
            //Arrange
            ProcessedLogs pLogs = new ProcessedLogs();
            List<string> url = new List<string>();
            for (int i = 0; i < 255000 / 255; i++)
            {
                for (int y = 0; y <= 255; y++)
                {
                    url.Add(string.Format("/Test/Test{0}/{1}", i, y));
                }
            }
            List<string> ipAddress = new List<string>();
            for (int i = 0; i < 255000 / 255; i++)
            {
                for (int y = 0; y <= 255; y++)
                {
                    ipAddress.Add(string.Format("192.168.{0}.{1}", i, y));
                }
            }
            //Act
            Parallel.ForEach(ipAddress, x => { pLogs.AddIpAddress(x); });
            Parallel.ForEach(url, x => { pLogs.AddURL(x); });
            //Assert
            Assert.IsTrue(pLogs.GetUrls().Count == 256000);            
            Assert.IsTrue(pLogs.GetIpAddresses().Count == 256000);
            
        }
    }
}
