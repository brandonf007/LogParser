using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace LogParser.Classes
{
    public class ApplicationConfig
    {
        public static string OutputFileName()
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings["OutputFileName"]) ? "Output.txt" : ConfigurationManager.AppSettings["OutputFileName"];
        }

        public static bool AppendToOutputFile()
        {
            if (bool.TryParse(ConfigurationManager.AppSettings["OutputFileName"], out bool result))
                return result;
            else
                return false;
        }

        public static bool DisplayUniqueIPs()
        {
            if (bool.TryParse(ConfigurationManager.AppSettings["DisplayUniqueIPs"], out bool result))
                return result;
            else
                return true;
        }

        public static bool DisplayUniqueURLs()
        {
            if (bool.TryParse(ConfigurationManager.AppSettings["DisplayUniqueURLs"], out bool result))
                return result;
            else
                return false;
        }

        public static int NumberToDisplay()
        {
            if (int.TryParse(ConfigurationManager.AppSettings["NumberToDisplay"], out int result))
                return result;
            else
                return 3;
        }
    }
}
