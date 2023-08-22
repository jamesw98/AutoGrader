using GraderCommon;
using GraderCommon.Enums;
using GraderCommon.Exceptions;
using GraderServices;

var grader = new Grader();
var info = new GradingInfo
{
    AssignmentName = "Sample Assignment",
    StudentFilesLocation = @"F:\dev\AutoGrader\samples\sample-1\student-files\",
    SolutionFilename = @"F:\dev\AutoGrader\samples\sample-1\sample-expected-output.txt",
    LanguageInfo = new LanguageInfo
    {
        LanguageType = LanguageType.Interpreted,
        Interpreter = "python",
        SetupArguments = new List<string>(),
        ExecutableArguments = new List<string>()
    },
    MaxPoints = 90,
    PointsPerLine = 10,
    Timeout = 30,
    CollapseWhiteSpace = false,
    CollapseNewLines = false,
    CaseSensitive = false,
    ReferenceFileLocation = null,
    StudentOutputType = OutputType.Stdout
};

try
{
    var reports = grader.Grade(info);
    Console.WriteLine(Path.GetTempPath());
}
catch (GraderException ge)
{
    Console.WriteLine($"Grader encountered an error:\n>    {ge.Message}");
}
