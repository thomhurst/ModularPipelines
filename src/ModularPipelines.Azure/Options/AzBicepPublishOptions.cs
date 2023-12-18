using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "publish")]
public record AzBicepPublishOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--target")] string Target
) : AzOptions
{
    [CommandSwitch("--documentationUri")]
    public string? DocumentationUri { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}

