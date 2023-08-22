using GraderCommon.Enums;
using GraderCommon.Exceptions;

namespace GraderCommon;

public class LanguageInfo
{
    /// <summary>
    /// what type of language is being used (compiled, interpreted, etc)
    /// </summary>
    public required LanguageType LanguageType { get; set; }

    /// <summary>
    /// if the language is compiled, the compiler to use
    /// </summary>
    public string? Compiler { get; set; } 

    /// <summary>
    /// if the language is interpreted, the interpreter to use
    /// </summary>
    public string? Interpreter { get; set; }
    
    /// <summary>
    /// arguments for compilation/interpretation 
    /// </summary>
    public required List<string> SetupArguments { get; set; }
    
    /// <summary>
    /// if the language is compiled, the name of the executable that is created after compilation
    /// </summary>
    public string? ExecutableName { get; set; }
    
    /// <summary>
    /// arguments for the executable
    /// </summary>
    public required List<string> ExecutableArguments { get; set; }
    
    /// <summary>
    /// how the programs get input
    /// </summary>
    public InputType InputType { get; set; }
    
    /// <summary>
    /// if the InputType is file, this is the file to input
    /// </summary>
    public string? InputFilename { get; set; }
    
    /// <summary>
    /// validates that the info give is properly formatted
    /// </summary>
    /// <exception cref="GraderArgException"></exception>
    public void ValidateLanguageInfo()
    {
        switch (LanguageType)
        {
            case LanguageType.Compiled when Compiler == null && ExecutableName == null:
                throw new GraderArgException("Compiled language type was selected, but no compiler was specified");
            case LanguageType.Interpreted when Interpreter == null:
                throw new GraderArgException("Interpreted language type was selected, but no interpreter was specified");
        }

        if (InputType == InputType.File && InputFilename == null)
        {
            throw new GraderArgException("File input type was selected, but no input file was specified");
        }
    }
}