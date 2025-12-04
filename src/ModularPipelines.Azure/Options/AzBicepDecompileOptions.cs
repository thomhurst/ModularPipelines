using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("bicep", "decompile")]
public record AzBicepDecompileOptions(
[property: CliOption("--file")] string File
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}