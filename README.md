# LogParser
The following C# Console Application can be used to Parse Logs, it will single out IP Addresses and URLs from the Logs and report on those accordingly.

## Quick Start Guide
Use the below to get instructions on what flags can be passed to the console application, after you compile the application you will be able to use the following instructions to run the LogParser.exe application
```cmd
LogParser.exe -h
```

The following is the basic way to run the Log Parser, -f flag is a required flag when trying to run the console application
```cmd
LogParser.exe -f filepath/filename.log
```

If the Parser is not returning expected results it runs by using regular expression to read a .log file line for line, the regular expression can be updated by passing an additional flag followed by the updated regular expression, the regular expression is expected to have two capturing groups where the first identifies the IP Address and the second one identifies the URL
```cmd
LogParser.exe -f filepath/filename.log -r "\.*(IPAddress)\.*(URL)\.*"
```
The regular expression provided above is just an example and cannot be used to run the Console Application

Default output of the Console Application is to the console, if you want to write the result in a text file, you will be required to provide the -o flag with an existing directory (defaults to Output.txt)
```cmd
LogParser.exe -f filepath/filename.log -o /filePath/existingDirectory
```

See [Detailed Guide](https://github.com/brandonf007/LogParser/wiki) on how to use the library in more depth.

NOTE: If the LogParser.UnitTest fails to build and complains about references, ensure that you build LogParser project first, following that you should be able to build the LogParser.UnitTest project, if it still complains about references you may need to remove the existing reference to LogParser under assemblies and readd that reference by right clicking on LogParser.UnitTest->Add->Project Reference and select the the LogParser.dll you built in the first step.
