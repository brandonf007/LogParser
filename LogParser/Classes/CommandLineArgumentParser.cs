using CommandLineParser.Arguments;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace LogParser
{
    /// <summary>
    /// Using a prebuilt command line argument parser, for more information regarding the parser refer to https://github.com/j-maly/CommandLineParser/wiki
    /// </summary>
    public class CommandLineArgumentParser
    {
        /// <summary>
        /// Provided command line arguments from the console application it governs the execution of the application
        /// </summary>
        /// <param name="args">Arguments passed to the console application through flags</param>
        /// <param name="outputPath">Should an output flag be passed this is where the output of the application will be written to</param>
        /// <param name="regularExpression">The regular expression to evaluate the log file per line</param>
        /// <param name="filePath">The path to the file which will be analysed by the Console Application</param>
        /// <returns></returns>
        public static bool ParseCommandLineArguments(string[] args, out string outputPath, out string regularExpression, out string filePath)
        {
            CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();
            ExpectedFlags flags = new ExpectedFlags();
            parser.ExtractArgumentAttributes(flags);
            string defaultRegex = @"(^(?:[0-9]{1,3}\.){3}[0-9]{1,3})(?:.+)(?:""{1}(?:GET|HEAD|POST|PUT|DELETE|CONNECT|OPTIONS|TRACE){1}\s{1}(.+)\s{1}(?:HTTP/1.1|HTTP/1.0|HTTP/2|HTTP/3){1}""{1})(?:.+$)";            
            try
            {
                parser.ParseCommandLine(args);

                if (flags.help)
                {
                    filePath = "";
                    outputPath = null;
                    regularExpression = defaultRegex;
                    ShowHelp(parser);
                    return false;
                }
                

                if (string.IsNullOrEmpty(flags.outputFile))
                    outputPath = null;
                else
                    outputPath = flags.outputFile;

                if(string.IsNullOrEmpty(flags.regexPattern))
                    regularExpression = defaultRegex;
                else
                    regularExpression = flags.regexPattern;

                filePath = flags.filePath;

                // Validate Output folder exists and is a folder
                if (!string.IsNullOrEmpty(flags.outputFile) && !Directory.Exists(outputPath))
                    throw new Exception("The output path does not point to an existing directory");

                // Validate the file exists
                if (!File.Exists(filePath))
                    throw new Exception(string.Format("The file provided does not exist: {0}", filePath));

                // Validate that the Regular Expression pattern provided is valid and not empty
                if (!isValidRegularExpression(regularExpression))
                    throw new Exception(string.Format("Provided Regular Expression pattern can not be empty, it should contain two groups the first identifying the IP Address, and the second identifying the URL"));

                return true;
            }
            catch (Exception ex)
            {
                if (flags.help)
                {
                    filePath = "";
                    outputPath = null;
                    regularExpression = defaultRegex;
                    ShowHelp(parser);
                    return false;
                }
                Console.WriteLine("There was an exception processing command line arguments passed, pass the -h or --help flag for more information regarding required flags");
                throw ex;
            }
        }
        /// <summary>
        /// Ensure that the Regular Expression Pattern provided is a valid expression as well as not empty
        /// </summary>
        /// <param name="regexPattern">The regulare expression to validate</param>
        /// <returns></returns>
        private static bool isValidRegularExpression(string regexPattern)
        {
            if (string.IsNullOrWhiteSpace(regexPattern)) return false;

            try
            {
                Regex.Match("", regexPattern);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return true;
        }
        /// <summary>
        /// Display help options to console
        /// </summary>
        /// <param name="parser">The CommandLineParser used to parse the arguments sent to the console application</param>
        private static void ShowHelp(CommandLineParser.CommandLineParser parser)
        {
            parser.ShowUsageHeader = "Here is how you use the console application: ";
            parser.ShowUsageFooter = "GLHF!";
            parser.ShowUsage();
        }
    }

    
    /// <summary>
    /// The various flags that the Console Application expects
    /// </summary>
    class ExpectedFlags
    {
        [SwitchArgument('h', "help", false, Description = "Use this flag to display the different command line arguments that can be passed to the application")]
        public bool help;
        [ValueArgument(typeof(string), 'o', "output", Description = "Provide the filepath to the Directory you would like to have the output generated as Output.txt, if this is not provided application will only output to console")]
        public string outputFile;
        [ValueArgument(typeof(string), 'r', "regularExpression", Description = "If Default Regular Expression pattern is not providing the correct output provide updated Regular Expression pattern by passing this flag followed by the pattern")]
        public string regexPattern;
        [ValueArgument(typeof(string), 'f', "filePath", Description = "Provide the filepath to the log file you require to analyse", Optional = false)]
        public string filePath;
    }
}
