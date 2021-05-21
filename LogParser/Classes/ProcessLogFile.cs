using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogParser
{
    /// <summary>
    /// This will process the profided log file, by reading the file line for line and match IP address and URL per line and store it to be processed further.
    /// </summary>
    public class ProcessLogFile
    {
        private string file;
        private string regexPattern { get; set; }
        private ProcessedLogs pLog { get; set; }
        private int totalNumberOfLines { get; set; }
        public int successful { get; set; }
        public int failed { get; set; }

        public ProcessLogFile(string file, string regexPattern, ProcessedLogs pLog)
        {
            this.successful = 0;
            this.failed = 0;
            this.file = file;
            this.regexPattern = regexPattern;
            this.pLog = pLog;
        }
        /// <summary>
        /// This will read a file line for line and send it along to RegexMatchLine to be processed
        /// </summary>
        public void ReadFromFileAndStore()
        {
            // used the following article to decide on the best way forward for reading large files https://cc.davelozinski.com/c-sharp/the-fastest-way-to-read-and-process-text-files
            try
            {
                var AllLines = File.ReadAllLines(file);
                totalNumberOfLines = AllLines.Length;
                Parallel.For(0, AllLines.Length, line =>
                 {
                     RegexMatchLine(AllLines[line]);
                 });

                successful = Math.Min(pLog.numberOfIpAddresses,pLog.numberOfURLs);
                failed = totalNumberOfLines - successful;
            }
            catch(AggregateException ex)
            {
                if (ex.InnerExceptions.All(x => x.Message == "Regex should match and return two capturing groups per line read in"))
                    throw new Exception("Regex should match and return two capturing groups per line read in");
                else
                    throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// This will match a line according to the provided Regular Expression, it expects two Capturing Groups the first to capture IP Address, and the second to capture URL in a given line
        /// with these groups it will store the extracted groups in two separate dictionaries <see cref="ProcessedLogs"/>
        /// </summary>
        /// <param name="line">The line to match IP Address and URL</param>
        private void RegexMatchLine(string line)
        {
            try
            {
                if (line.Length > 0)
                {
                    Match match = Regex.Match(line, regexPattern,
                                                    RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        if (match.Groups.Count == 3)
                        {
                            pLog.AddIpAddress(match.Groups[1].Value);
                            pLog.AddURL(match.Groups[2].Value);
                        }
                        else
                            throw new Exception("Regex should match and return two capturing groups per line read in");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        
    }
}