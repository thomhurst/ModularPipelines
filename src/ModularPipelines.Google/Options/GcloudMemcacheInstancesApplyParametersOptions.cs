using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memcache", "instances", "apply-parameters")]
public record GcloudMemcacheInstancesApplyParametersOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region,
[property: BooleanCommandSwitch("--apply-all")] bool ApplyAll,
[property: CommandSwitch("--node-ids")] string[] NodeIds
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}