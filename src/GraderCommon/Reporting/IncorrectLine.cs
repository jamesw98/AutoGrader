namespace GraderCommon.Reporting;

/// <summary>
/// represents a line that does not match the expected output
/// </summary>
public class IncorrectLine
{
    /// <summary>
    /// optional custom message to be displayed above the stock error message
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// the student's line that is incorrect
    /// </summary>
    public required string StudentLine { get; set; }
    
    /// <summary>
    /// the line that was expected
    /// </summary>
    public required string ExpectedLine { get; set; }
    
    /// <summary>
    /// points subtracted from total score for this incorrect line
    /// </summary>
    public required int PointsSubtracted { get; set; }
}