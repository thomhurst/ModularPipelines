using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aosm", "nfd", "build")]
public record AzAosmNfdBuildOptions(
[property: CommandSwitch("--config-file")] string ConfigFile,
[property: CommandSwitch("--definition-type")] string DefinitionType
) : AzOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--order-params")]
    public bool? OrderParams { get; set; }
}