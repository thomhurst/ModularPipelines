using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "install")]
public record AzBicepInstallOptions : AzOptions
{
    [CommandSwitch("--target-platform")]
    public string? TargetPlatform { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}