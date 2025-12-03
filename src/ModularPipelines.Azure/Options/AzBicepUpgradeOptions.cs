using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bicep", "upgrade")]
public record AzBicepUpgradeOptions : AzOptions
{
    [CliOption("--target-platform")]
    public string? TargetPlatform { get; set; }
}