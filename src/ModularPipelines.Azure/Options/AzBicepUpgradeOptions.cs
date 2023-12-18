using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "upgrade")]
public record AzBicepUpgradeOptions : AzOptions
{
    [CommandSwitch("--target-platform")]
    public string? TargetPlatform { get; set; }
}