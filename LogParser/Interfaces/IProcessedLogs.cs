using System.Collections.Generic;

namespace LogParser
{
    internal interface IProcessedLogs
    {        
        public void AddURL(string url);
        public void AddIpAddress(string ipAddress);
        public Dictionary<string, int> GetIpAddresses();
        public Dictionary<string, int> GetUrls();
    }
}