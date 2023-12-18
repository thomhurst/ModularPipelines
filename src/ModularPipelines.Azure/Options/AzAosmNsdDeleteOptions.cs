using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aosm", "nsd", "delete")]
public record AzAosmNsdDeleteOptions(
[property: CommandSwitch("--config-file")] string ConfigFile
) : AzOptions
{
    [BooleanCommandSwitch("--clean")]
    public bool? Clean { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}