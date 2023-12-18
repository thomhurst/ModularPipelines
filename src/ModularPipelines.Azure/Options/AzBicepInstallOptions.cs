using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "install")]
public record AzBicepInstallOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [CommandSwitch("--target-platform")]
    public string? TargetPlatform { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}