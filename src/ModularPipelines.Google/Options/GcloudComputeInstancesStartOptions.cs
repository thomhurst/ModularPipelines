using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "start")]
public record GcloudComputeInstancesStartOptions(
[property: PositionalArgument] string InstanceNames
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}