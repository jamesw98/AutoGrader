using System.Diagnostics;
using GraderCommon;
using GraderCommon.Enums;
using GraderCommon.Exceptions;

namespace GraderServices;

public static class ProcessRunner
{
    /// <summary>
    /// run an interpreted solution
    /// </summary>
    /// <param name="command">the command to run</param>
    /// <param name="info">grading info</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="GraderArgException"></exception>
    public static IEnumerable<string> RunInterpreted(string command, GradingInfo info)
    {
        info.ValidateInfo();
        return info.StudentOutputType switch
        {
            OutputType.Stdout => 
                RunStdout(info.LanguageInfo.Interpreter!, info.StudentFilesLocation, command),
            OutputType.File => 
                throw new NotImplementedException("File output not supported currently"),
            _ => throw new GraderArgException("Unknown output type")
        };
    }

    /// <summary>
    /// runs a process and will grab the output from stdout
    /// </summary>
    /// <param name="command">the base command to run ie: dotnet, python, java, etc</param>
    /// <param name="pathToFile">the path to the file to run</param>
    /// <param name="fileName">the name of the file to run</param>
    /// <returns></returns>
    private static IEnumerable<string> RunStdout(string command, string pathToFile, string fileName)
    {
        // if there is a path separator character at the end of the path to the file, add one
        if (pathToFile.Last() != Path.DirectorySeparatorChar)
        {
            pathToFile += Path.DirectorySeparatorChar;
        }
        
        // setup
        var result = new List<string>();
        var proc = SetupProcess(command, pathToFile, fileName, OutputType.Stdout, result);
        proc.OutputDataReceived += (_, args) => ProcessStdout(result, args.Data);
    
        // run the process
        proc.Start();
        proc.BeginOutputReadLine();
        proc.WaitForExit();
        
        return result;
    }
    
    /// <summary>
    /// sets up a process to run 
    /// </summary>
    /// <param name="command">the base command to run ie: dotnet, python, java, etc</param>
    /// <param name="pathToFile">the path to the file to run</param>
    /// <param name="fileName">the name of the file to run</param>
    /// <param name="outputType">the expected output type</param>
    /// <param name="results">a collection of strings that stdout will be written to</param>
    /// <returns></returns>
    private static Process SetupProcess
    (
        string command,
        string pathToFile,
        string fileName,
        OutputType outputType,
        ICollection<string> results
    )
    {
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                Arguments = $"{pathToFile}{fileName}",
                FileName = command
            },
        };

        return proc;
    }
    
    /// <summary>
    /// processes basic stdout
    /// </summary>
    /// <param name="result"></param>
    /// <param name="line"></param>
    private static void ProcessStdout(ICollection<string> result, string? line)
    {
        if (!string.IsNullOrEmpty(line))
        {
            result.Add(line);
        }
    }
}