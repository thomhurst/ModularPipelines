using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aosm", "nfd", "delete")]
public record AzAosmNfdDeleteOptions(
[property: CommandSwitch("--config-file")] string ConfigFile,
[property: CommandSwitch("--definition-type")] string DefinitionType
) : AzOptions
{
    [BooleanCommandSwitch("--clean")]
    public bool? Clean { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}

