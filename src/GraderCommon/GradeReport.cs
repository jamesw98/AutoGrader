namespace GraderCommon;

public class GradeReport
{
    /// <summary>
    /// unique id for this grade report
    /// </summary>
    public required long ReportId { get; set; }
    
    /// <summary>
    /// can be a PID, email address, name, etc
    /// </summary>
    public required string StudentId { get; set; }
    
    /// <summary>
    /// the points the student scored for this report
    /// </summary>
    public required int ScoredPoints { get; set; }
    
    /// <summary>
    /// the max total points for this report 
    /// </summary>
    public required int MaxPoints { get; set; }

    /// <summary>
    /// the lines of output the student's code produced
    /// </summary>
    public List<string>? StudentLines { get; set; }
    
    /// <summary>
    /// only used if the student's code encountered an exception  
    /// </summary>
    public string? StudentException { get; set; }
    
    /// <summary>
    /// list of incorrect lines from the student's output
    /// </summary>
    public required List<IncorrectLine> IncorrectLines { get; set; }
}