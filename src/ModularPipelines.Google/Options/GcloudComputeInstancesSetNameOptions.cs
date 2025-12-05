using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "set-name")]
public record GcloudComputeInstancesSetNameOptions(
[property: CliArgument] string InstanceName,
[property: CliOption("--new-name")] string NewName
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}