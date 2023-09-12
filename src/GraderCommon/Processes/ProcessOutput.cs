using GraderCommon.Enums;

namespace GraderCommon.Processes;

public class ProcessOutput
{
    public required string Filename { get; set; }
    public required OutputType OutputType { get; set; }
    public required ICollection<string> Output { get; set; }
    public required ProcessResult ProcessResult { get; set; }
    public string? ExceptionMessage { get; set; }
}