using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "restore")]
public record AzBicepRestoreOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}

