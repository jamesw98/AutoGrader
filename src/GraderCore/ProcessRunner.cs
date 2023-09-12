using System.Diagnostics;
using GraderCommon;
using GraderCommon.Enums;
using GraderCommon.Exceptions;
using GraderCommon.Processes;
using GraderCommon.SetupInfo;

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
    public static ProcessOutput RunInterpreted(string command, GradingInfo info)
    {
        info.ValidateInfo();
        return info.StudentOutputType switch
        {
            OutputType.Stdout =>
                RunStdout(info.LanguageInfo.Interpreter!, info.StudentFilesLocation, command, info.Timeout),
            OutputType.File =>
                throw new NotImplementedException("File output not supported currently"),
            _ => throw new GraderArgException("Unknown output type")
        };
    }

    private static ProcessOutput RunStdout(string command, string pathToFile, string fileName, int timeout)
    {
        var stdoutResult = new ProcessOutput
        {
            Filename = fileName,
            Output = new List<string>(),
            ProcessResult = ProcessResult.Exited,
            OutputType = OutputType.Stdout
        };
        
        Console.WriteLine($"Running {fileName}");
        // if there is a path separator character at the end of the path to the file, add one
        if (pathToFile.Last() != Path.DirectorySeparatorChar)
        {
            pathToFile += Path.DirectorySeparatorChar;
        }

        // setup
        var proc = SetupProcess(command, pathToFile, fileName, OutputType.Stdout);
        proc.OutputDataReceived += (_, args) => ProcessStdout(stdoutResult.Output, args.Data);

        // run the process
        proc.Start();
        proc.BeginOutputReadLine();
        proc.WaitForExit(TimeSpan.FromSeconds(timeout));
        
        // if the process hasn't exited, then it timed out
        if (!proc.HasExited)
        {
            stdoutResult.ProcessResult = ProcessResult.TimedOut;
            return stdoutResult;
        }
        
        // if the process has a non-0 exit code, an exception occured, read from stderr and log the message
        if (proc.ExitCode != 0)
        {
            stdoutResult.ProcessResult = ProcessResult.Exception;
            stdoutResult.ExceptionMessage = proc.StandardError.ReadToEnd();
            return stdoutResult;
        }
        
        return stdoutResult;
    }

    /// <summary>
    /// sets up a process to run 
    /// </summary>
    /// <param name="command">the base command to run ie: dotnet, python, java, etc</param>
    /// <param name="pathToFile">the path to the file to run</param>
    /// <param name="fileName">the name of the file to run</param>
    /// <param name="outputType">the expected output type</param>
    /// <returns></returns>
    private static Process SetupProcess
    (
        string command,
        string pathToFile,
        string fileName,
        OutputType outputType
    )
    {
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
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