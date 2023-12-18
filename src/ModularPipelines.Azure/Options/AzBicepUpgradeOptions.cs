using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "upgrade")]
public record AzBicepUpgradeOptions : AzOptions
{
    [CommandSwitch("--target-platform")]
    public string? TargetPlatform { get; set; }
}