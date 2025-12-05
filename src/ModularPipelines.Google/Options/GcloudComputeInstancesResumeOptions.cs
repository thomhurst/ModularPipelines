using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "resume")]
public record GcloudComputeInstancesResumeOptions(
[property: CliArgument] string InstanceNames
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}