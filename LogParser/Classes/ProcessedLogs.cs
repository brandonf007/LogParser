using System.Collections.Generic;
using System.Threading;

namespace LogParser
{
    /// <summary>
    /// Class to store the URLs and IPAdresses extracted per line read from the provided file, it will also track the number
    /// of successful and failed lines to report back to user if lines do fail or do not match the provided Regular Expression
    /// pattern
    /// </summary>
    public class ProcessedLogs : IProcessedLogs 
    {
        private static Mutex ipMutex = new Mutex();
        private static Mutex urlMutex = new Mutex();
        // uniquely store urls, whenever there is a clash increase value
        private Dictionary<string, int> urls { get; set; }
        public int numberOfURLs;
        // uniquely store ipAddress, whenever there is a clash increase value
        private Dictionary<string, int> ipAddresses { get; set; }
        public int numberOfIpAddresses;

        public ProcessedLogs()
        {
            urls = new Dictionary<string, int>();
            ipAddresses = new Dictionary<string, int>();
            numberOfURLs = 0;
            numberOfIpAddresses = 0;
        }
        /// <summary>
        /// Add IPAdress to dictionary to uniquely store IP Addresses, if a clash occurs the IP Adress value will be increased by 1
        /// </summary>
        /// <param name="ipAddress">The IP Address to add to the dictionary</param>
        public void AddIpAddress(string ipAddress)
        {
            ipMutex.WaitOne();
            numberOfIpAddresses++;
            int ipCount = 0;
            if (ipAddresses.TryGetValue(ipAddress, out ipCount))
            {
                ipAddresses[ipAddress] = ipCount + 1;
            }
            else
            {
                ipAddresses.Add(ipAddress, 1);
            }
            ipMutex.ReleaseMutex();
        }
        /// <summary>
        /// Add URL to dictionary to uniquely store URLs, if a clash occurs the URL value will be increased by 1
        /// </summary>
        /// <param name="url">The URL to add to the dictionary</param>
        public void AddURL(string url)
        {
            urlMutex.WaitOne();
            numberOfURLs++;
            int urlCount = 0;
            if (urls.TryGetValue(url, out urlCount))
            {
                urls[url] = urlCount + 1;
            }
            else
            {
                urls.Add(url, 1);
            }
            urlMutex.ReleaseMutex();
        }

        public Dictionary<string, int> GetIpAddresses()
        {
            return ipAddresses;
        }

        public Dictionary<string, int> GetUrls()
        {
            return urls;
        }
    }
}
