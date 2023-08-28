using GraderCommon.SetupInfo;

namespace GraderCommon.Reporting;

public class BatchReport
{
    /// <summary>
    /// optional:
    /// the grading setup info used to run this batch
    /// </summary>
    public GradingInfo? GradingInfo { get; set; }
    
    /// <summary>
    /// reports for each submission that was graded
    /// </summary>
    public List<SubmissionReport> SubmissionReports { get; set; }
    
    /// <summary>
    /// the average score of all the submissions
    /// </summary>
    public double AverageScore { get; set; }
    
}