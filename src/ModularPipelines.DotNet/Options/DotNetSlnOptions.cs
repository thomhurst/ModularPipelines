using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetSlnOptions : DotNetOptions
{
    public DotNetSlnOptions(
        string solutionFile
    )
    {
        CommandParts = ["sln", "[<SOLUTION_FILE>]", "[command]"];

        SolutionFile = solutionFile;
    }

    public DotNetSlnOptions()
    {
        CommandParts = ["sln", "[<SOLUTION_FILE>]", "[command]"];
    }

    [CliArgument(Name = "[<SOLUTION_FILE>]")]
    public virtual string? SolutionFile { get; set; }
}
