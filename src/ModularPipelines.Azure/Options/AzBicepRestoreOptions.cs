using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bicep", "restore")]
public record AzBicepRestoreOptions(
[property: CliOption("--file")] string File
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}