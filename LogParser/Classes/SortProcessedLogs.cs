using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogParser
{
    /// <summary>
    /// Used to sort IP Dictionary and URL Dictionary into two lists sorted in a descending order
    /// </summary>
    public class SortProcessedLogs : ISortProcessedLogs
    {
        private ProcessedLogs pLogs;
        public List<KeyValuePair<string, int>> sortedIPList { get; set; }
        public List<KeyValuePair<string, int>> sortedUrlList { get; set; }
        public SortProcessedLogs(ProcessedLogs pLogs)
        {
            sortedUrlList = new List<KeyValuePair<string, int>>();
            sortedIPList = new List<KeyValuePair<string, int>>();
            this.pLogs = pLogs;                        
        }
        /// <summary>
        /// This will begin the process of sorting the two dictionaries containing the URLs and IP Addresses
        /// </summary>
        /// <param name="asc">Defaults to descending but if true will sort the dictionaries in ascending order</param>
        public void BeginSort(bool asc = false, bool thenBy = false)
        {
            Task sortURL = Task.Factory.StartNew(() => TaskSortURL(asc, thenBy));
            Task sortIp = Task.Factory.StartNew(() => TaskSortIP(asc, thenBy));
            Task.WaitAll(sortIp,sortURL);
        }
        /// <summary>
        /// Task to sort the URL Dictionary
        /// </summary>
        /// <param name="asc">Defaults to descending but if true will sort the dictionary in ascending order</param>
        private void TaskSortURL(bool asc, bool thenBy = false)
        {
            if (asc)
            {
                if(!thenBy)
                    sortedUrlList = pLogs.GetUrls().AsParallel().OrderBy(d => d.Value).ToList();
                else
                    sortedUrlList = pLogs.GetUrls().AsParallel().OrderBy(d => d.Value).ThenBy(x => x.Key).ToList();
            }
            else
            {
                if(!thenBy)
                    sortedUrlList = pLogs.GetUrls().AsParallel().OrderByDescending(d => d.Value).ToList();
                else
                    sortedUrlList = pLogs.GetUrls().AsParallel().OrderByDescending(d => d.Value).ThenByDescending(x => x.Key).ToList();
            }
                
        }
        /// <summary>
        /// Task to sort the IP Address dictionary
        /// </summary>
        /// <param name="asc">Defaults to descending but if true will sort the dictionary in ascending order</param>
        private void TaskSortIP(bool asc, bool thenBy = false)
        {
            if (asc)
            {
                if(!thenBy)
                    sortedIPList = pLogs.GetIpAddresses().AsParallel().OrderBy(d => d.Value).ToList();
                else                    
                    sortedIPList = pLogs.GetIpAddresses().AsParallel().OrderBy(d => d.Value).ThenBy(x => x.Key).ToList();
            }
            else
            {
                if(!thenBy)
                    sortedIPList = pLogs.GetIpAddresses().AsParallel().OrderByDescending(d => d.Value).ToList();
                else
                    sortedIPList = pLogs.GetIpAddresses().AsParallel().OrderByDescending(d => d.Value).ThenByDescending(x => x.Key).ToList();
            }
                
        }
    }
}