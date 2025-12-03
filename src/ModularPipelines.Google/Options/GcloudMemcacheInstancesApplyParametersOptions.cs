using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "instances", "apply-parameters")]
public record GcloudMemcacheInstancesApplyParametersOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region,
[property: CliFlag("--apply-all")] bool ApplyAll,
[property: CliOption("--node-ids")] string[] NodeIds
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}