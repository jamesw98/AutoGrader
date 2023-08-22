using GraderCommon.Enums;

namespace GraderCommon;

public class ReferenceInfo
{
    /// <summary>
    /// file name for the reference solution executable
    /// </summary>
    public required string ReferenceExecutableName { get; set; }
    
    /// <summary>
    /// output type for the reference solution
    /// </summary>
    public required OutputType ReferenceOutputType { get; set; }
    
    /// <summary>
    /// the filename for the output
    /// if ReferenceOutputType is File
    /// </summary>
    public string? OutputFilename { get; set; } 
}