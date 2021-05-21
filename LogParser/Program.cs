using LogParser.Classes;
using System;
using System.Configuration;

namespace LogParser
{
    class Program
    {        
        /// <summary>
        /// This is where the program will start and it will begin by reading in command line arguments passed and setting the 
        /// properties, where expected values are
        /// -h --help - Use this flag to display the different command line arguments that can be passed to the application
        /// -o --output - Provide the filepath to the Directory you would like to have the output generated as Output.txt, if this is not provided application will only output to console
        /// -f --filePath - Provide the filepath to the log file you require to analyse
        /// -r --regularExpression - If Default Regular Expression pattern is not providing the correct output provide updated Regular Expression pattern by passing this flag followed by the pattern
        /// </summary>
        /// <param name="args">Command Line Arguments passed</param>
        static void Main(string[] args)
        {
            string file = "";        
            string outputPath = "";            
            string regexPattern = "";

            Console.WriteLine("-- Log Parser Start --");
            try
            {                
                // Only run if the -h --help flags are not passed as arguments
                if(CommandLineArgumentParser.ParseCommandLineArguments(args, out outputPath, out regexPattern, out file))
                {
                    ProcessedLogs pLogs = new ProcessedLogs();
                    // Process the log files
                    ProcessLogFile logger = new ProcessLogFile(file, regexPattern, pLogs);
                    logger.ReadFromFileAndStore();
                    // Sort the processed logs
                    SortProcessedLogs sort = new SortProcessedLogs(pLogs);
                    sort.BeginSort();
                    // Output the results either to console, or to ouput file
                    OutputResults.PrintToConsole(sort.sortedIPList, sort.sortedUrlList, numberOfResults: ApplicationConfig.NumberToDisplay(),
                                                    displayUniqueIP: ApplicationConfig.DisplayUniqueIPs(),displayUniqueURL: ApplicationConfig.DisplayUniqueURLs());

                    if (!string.IsNullOrEmpty(outputPath))
                        OutputResults.PrintToFile(sort.sortedIPList, sort.sortedUrlList, outputFilepath:outputPath,
                                                    fileName: ApplicationConfig.OutputFileName(),numberOfResults: ApplicationConfig.NumberToDisplay(),
                                                    append: ApplicationConfig.AppendToOutputFile(), displayUniqueIP: ApplicationConfig.DisplayUniqueIPs(),
                                                    displayUniqueURL: ApplicationConfig.DisplayUniqueURLs());
                }
                Console.WriteLine("-- Log Parser End --");

            }
            catch(Exception ex)
            {
                Console.WriteLine("Something went wrong during execution, refer to exception for additional information");
                Console.WriteLine(ex.Message);
                Console.WriteLine("-- Log Parser End --");
            }
        }
    }
}
