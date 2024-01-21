using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("sln", "[<SOLUTION_FILE>]", "[command]")]
[ExcludeFromCodeCoverage]
public record DotNetSlnOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "[<SOLUTION_FILE>]")]
    public string? SolutionFile { get; set; }
}
