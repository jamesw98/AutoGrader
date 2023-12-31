﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using GraderCommon;
using GraderCommon.Enums;
using GraderCommon.Exceptions;

namespace GraderServices;

public partial class Grader
{
    [GeneratedRegex("\\s+")]
    private static partial Regex MultipleSpaces();
    
    /// <summary>
    /// grades all submissions in studentBlobStorageLocation
    /// </summary>
    /// <param name="info">all information needed to grade submissions</param>
    /// <returns></returns>
    public List<GradeReport> Grade(GradingInfo info)
    {
        var tempGradingDirName = $"{info.AssignmentName}-{DateTime.UtcNow.ToFileTime()}";
        
        // ensure the info object is properly setup
        info.ValidateInfo();
        
        // get the expected output
        var expectedOutput = GetSolutionLines(info);
        
        // run the student code and generate reports
        var reports = GradeStudentFiles(info, expectedOutput);
        return reports;
    }
    
    /// <summary>
    /// grades student files
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private List<GradeReport> GradeStudentFiles(GradingInfo info, IEnumerable<string> expectedOutput)
    {
        var results = new List<GradeReport>();
        
        // get the file names (not paths, just names) to grade
        var files = Directory
            .GetFiles(info.StudentFilesLocation)
            .Select(x => x.Split(Path.DirectorySeparatorChar).Last());
        
        // grade each file
        foreach (var file in files)
        {
            var studentOutput = ProcessRunner.RunInterpreted(file, info);
            var report = CompareStudentOutputToExpected(studentOutput.ToList(), expectedOutput, info);
            results.Add(report);
        }

        return results;
    }
    
    /// <summary>
    /// generate a grade report for a student's output
    /// </summary>
    /// <param name="studentOutput">student's outputted lines</param>
    /// <param name="expectedOutput">expected outputted lines</param>
    /// <param name="info">grading info</param>
    /// <returns>a grading report</returns>
    private static GradeReport CompareStudentOutputToExpected
    (
        List<string> studentOutput,
        IEnumerable<string> expectedOutput,
        GradingInfo info
    )
    {
        var score = 0 + info.FreePoints ?? 0;
        var incorrectLine = new List<IncorrectLine>();
        
        expectedOutput.Each((expected, i) =>
        {
            // this feels kinda gross, but it works
            if (i > studentOutput.Count - 1)
            {
                incorrectLine.Add(new IncorrectLine
                {
                    ErrorMessage = "No line found for expected line",
                    StudentLine = "",
                    ExpectedLine = expected,
                    PointsSubtracted = info.PointsPerLine
                });
            }
            else if (!expected.Trim().Equals(studentOutput[i].Trim()))
            {
                incorrectLine.Add(new IncorrectLine
                {
                    StudentLine = studentOutput[i],
                    ExpectedLine = expected,
                    PointsSubtracted = info.PointsPerLine
                });
            }
            else
            {
                score += info.PointsPerLine;
            }
        });

        return new GradeReport
        {
            ReportId = 1,
            StudentId = Guid.NewGuid().ToString(),
            ScoredPoints = score,
            MaxPoints = info.MaxPoints,
            IncorrectLines = incorrectLine,
            StudentLines = studentOutput
        };
    }
    
    /// <summary>
    /// gets solution lines from either running a reference solution or from a static solution file
    /// </summary>
    /// <param name="info">the grading info</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private IEnumerable<string> GetSolutionLines(GradingInfo info)
    {
        if (info.ReferenceInfo != null)
        {
            // run reference executable against input, get the output, return the output
            return GetSolutionFromReferenceSolution(info.ReferenceInfo);
        }

        if (info.SolutionFilename != null)
        {
            return GetSolutionFromFile
            (
                info.SolutionFilename,
                info.CollapseWhiteSpace,
                info.CollapseNewLines
            );
        }

        throw new ArgumentException("No reference solution or solution file found, check your configuration");
    }
    
    /// <summary>
    /// gets expected output lines after running a reference solution
    /// </summary>
    /// <param name="info">relevant grading info</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static IEnumerable<string> GetSolutionFromReferenceSolution(ReferenceInfo info)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// gets expected output from a given solution file, no running a reference solution necessary
    /// </summary>
    /// <param name="solutionFilename"></param>
    /// <param name="collapseWhiteSpace"></param>
    /// <param name="collapseNewLines"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    private static IEnumerable<string> GetSolutionFromFile
    (
        string solutionFilename, 
        bool collapseWhiteSpace, 
        bool collapseNewLines
    )
    {
        solutionFilename = solutionFilename.FixPathSeparators();
        
        // ensure the file exists
        if (!File.Exists(solutionFilename))
        {
            throw new GraderFileNotFoundException($"Could not find file {solutionFilename}");
        }
        
        // get the lines
        var lines = File.ReadAllLines(solutionFilename)
            .AsEnumerable();
        
        // if selected, collapse all whitespace
        if (collapseWhiteSpace)
        {
            lines = lines.Select(l => MultipleSpaces().Replace(l, " "));
        }

        return lines;
    }
}