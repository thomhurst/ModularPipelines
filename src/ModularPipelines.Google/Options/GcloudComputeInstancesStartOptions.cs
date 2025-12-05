using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "start")]
public record GcloudComputeInstancesStartOptions(
[property: CliArgument] string InstanceNames
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}