using System;
using System.Collections.Generic;
using System.IO;

namespace LogParser
{
    /// <summary>
    /// This class handles output of results, always to console and if chosen to file
    /// </summary>
    public class OutputResults
    {
        /// <summary>
        /// Will print results to console
        /// </summary>
        /// <param name="sortedIPList">Sorted list of IP addresses</param>
        /// <param name="sortedUrlList">Sorted list of URLs</param>
        /// <param name="numberOfResults">How many of the 'top' results to print out</param>
        /// <param name="displayUniqueIP">Include output of number of unique IP Address</param>
        /// <param name="displayUniqueURL">Include output of number of unique URLs</param>
        public static void PrintToConsole(List<KeyValuePair<string, int>> sortedIPList, List<KeyValuePair<string, int>> sortedUrlList, int numberOfResults = 3, bool displayUniqueIP = true,bool displayUniqueURL = false)
        {
            try
            {
                Console.WriteLine(GenerateOutput(sortedIPList,sortedUrlList,numberOfResults,displayUniqueIP,displayUniqueURL));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("failed to write output to console"));
                throw ex;
            }
        }
        /// <summary>
        /// Will print results out to a text file Output.txt, if the file does not exist it will create it, if it does it will append to it
        /// </summary>
        /// <param name="sortedIPList">Sorted list of IP addresses</param>
        /// <param name="sortedUrlList">Sorted list of URLs</param>
        /// <param name="outputFilepath">The directory where the Output.txt will be printed out to</param>
        /// <param name="numberOfResults">How many of the 'top' results to print out</param>
        /// <param name="displayUniqueIP">Include output of number of unique IP Address</param>
        /// <param name="displayUniqueURL">Include output of number of unique URLs</param>
        /// <param name="append">Choose to append or overwrite file</param>
        public static void PrintToFile(List<KeyValuePair<string, int>> sortedIPList, List<KeyValuePair<string, int>> sortedUrlList, string outputFilepath, int numberOfResults = 3, bool displayUniqueIP = true, bool displayUniqueURL = false, bool append = true, string fileName = "Output.txt")
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(outputFilepath, fileName), append))
                {
                    outputFile.WriteLine(GenerateOutput(sortedIPList, sortedUrlList, numberOfResults, displayUniqueIP, displayUniqueURL));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(string.Format("failed to write output to file"));
                throw ex;
            }            
        }
        /// <summary>
        /// Will generate the results to print to console or text file
        /// </summary>
        /// <param name="sortedIPList">Sorted list of IP addresses</param>
        /// <param name="sortedUrlList">Sorted list of URLs</param>
        /// <param name="numberOfResults">How many of the 'top' results to print out</param>
        /// <param name="displayUniqueIP">Include output of number of unique IP Address</param>
        /// <param name="displayUniqueURL">Include output of number of unique URLs</param>
        /// <returns></returns>
        private static string GenerateOutput(List<KeyValuePair<string, int>> sortedIPList, List<KeyValuePair<string, int>> sortedUrlList, int numberOfResults, bool displayUniqueIP, bool displayUniqueURL)
        {
            string retVal = new string("");
            try
            {
                int z = 0;
                if (sortedIPList != null && sortedIPList.Count > 0)
                {
                    if (displayUniqueIP)
                        retVal += string.Format("Number of unique IP Addresses:{0}\r\n", sortedIPList.Count);
                    z = System.Math.Min(sortedIPList.Count, numberOfResults <= sortedIPList.Count ? numberOfResults : sortedIPList.Count);
                    for (int i = 0; i < z; i++)
                    {
                        retVal += string.Format("Most Active IP Addresses:{0} - HITS:{1}\r\n", sortedIPList[i].Key, sortedIPList[i].Value);
                    }
                }

                if (sortedUrlList != null && sortedUrlList.Count > 0)
                {
                    if (displayUniqueURL)
                        retVal += string.Format("Number of unique URLs:{0}\r\n", sortedUrlList.Count);
                    z = System.Math.Min(sortedUrlList.Count, numberOfResults <= sortedUrlList.Count ? numberOfResults : sortedUrlList.Count);
                    for (int i = 0; i < z; i++)
                    {
                        retVal += string.Format("Most visited URLS:{0} - Occurences:{1}\r\n", sortedUrlList[i].Key, sortedUrlList[i].Value);
                    }
                }
                return retVal;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
    }
}