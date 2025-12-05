using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-instances", "create")]
public record GcloudComputeTargetInstancesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--instance-zone")]
    public string? InstanceZone { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}