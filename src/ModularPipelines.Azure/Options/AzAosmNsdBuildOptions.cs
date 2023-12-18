using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aosm", "nsd", "build")]
public record AzAosmNsdBuildOptions(
[property: CommandSwitch("--config-file")] string ConfigFile
) : AzOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}

