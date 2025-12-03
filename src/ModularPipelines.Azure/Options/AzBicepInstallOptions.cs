using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bicep", "install")]
public record AzBicepInstallOptions : AzOptions
{
    [CliOption("--target-platform")]
    public string? TargetPlatform { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}