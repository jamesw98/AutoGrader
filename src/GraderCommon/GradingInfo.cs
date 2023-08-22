using GraderCommon.Enums;
using GraderCommon.Exceptions;

namespace GraderCommon;

public class GradingInfo
{
    /// <summary>
    /// the name of the assignment 
    /// </summary>
    public required string AssignmentName { get; set; }
    
    /// <summary>
    /// whether or not to write results to a local directory
    /// </summary>
    public bool UseLocalStorage { get; set; }
    
    /// <summary>
    /// where to write results if UseLocalStorage is true
    /// </summary>
    public string? LocalStorageLocation { get; set; }
    
    /// <summary>
    /// where student files are located
    /// </summary>
    public required string StudentFilesLocation { get; set; }
    
    /// <summary>
    /// output type for the student files
    /// </summary>
    public required OutputType StudentOutputType { get; set; }
    
    /// <summary>
    /// where reference files are located
    /// only required when ReferenceInfo is not null
    /// </summary>
    public string? ReferenceFileLocation { get; set; }
    
    /// <summary>
    /// information about the language being used
    /// </summary>
    public required LanguageInfo LanguageInfo { get; set; }

    /// <summary>
    /// optional: used when not using a reference solution
    /// name of the file to compare student output to
    /// if reference solution is not null, this will be ignored
    ///
    /// NOTE: this should probably only be used if the input for the program will always be the same
    /// </summary>
    public string? SolutionFilename { get; set; }
    
    /// <summary>
    /// optional: used when using a reference solution
    /// information about the reference solution (what to compare student output to)
    /// it will use the same language info as the student code
    /// </summary>
    public ReferenceInfo? ReferenceInfo { get; set; }
    
    /// <summary>
    /// the max possible points to be earned for this assignment
    /// </summary>
    public required int MaxPoints { get; set; }
    
    /// <summary>
    /// points given per correct line
    /// </summary>
    public required int PointsPerLine { get; set; }
    
    /// <summary>
    /// optional: points given regardless of grader results
    /// </summary>
    public int? FreePoints { get; set; }

    /// <summary>
    /// time before giving 0 (or FreePoints) points to the submission
    /// </summary>
    public required double Timeout { get; set; } = 0.30;

    /// <summary>
    /// collapses all whitespace to 1 space per line
    /// for example:
    ///     "this is      a   test      string"
    /// will become
    ///     "this is a test string" 
    /// </summary>
    public required bool CollapseWhiteSpace { get; set; } = true;

    /// <summary>
    /// collapses multiple blank newlines in a row into 1 new line
    /// for example:
    ///     "this is
    ///
    ///
    ///     a test"
    /// will become:
    ///     "this is
    ///
    ///     a test"
    /// </summary>
    public required bool CollapseNewLines { get; set; } = false;

    /// <summary>
    /// if true, case wil mater when comparing lines from reference to student outputs
    /// </summary>
    public required bool CaseSensitive { get; set; } = false;
    
    /// <summary>
    /// validate the grading info
    /// </summary>
    /// <exception cref="GraderArgException"></exception>
    public void ValidateInfo()
    {
        // check local storage options
        if (UseLocalStorage && LocalStorageLocation == null)
        {
            throw new GraderArgException("To use local storage you must specify a local storage location");
        }
    
        // check reference and solution options
        if (ReferenceInfo == null && SolutionFilename == null)
        {
            throw new GraderArgException("\"ReferenceInfo\" object or \"SolutionFilename\" must be included");
        }

        // ensure MaxPoints is set properly
        if (MaxPoints < 0)
        {
            throw new GraderArgException("\"MaxPoints\" cannot be less than 0");
        }
        
        // ensure PointsPerLine is set properly
        if (PointsPerLine < 1)
        {
            throw new GraderArgException("\"PointsPerLine\" cannot be less than 1");
        }
        
        // validate the language information
        LanguageInfo.ValidateLanguageInfo();
    }
}