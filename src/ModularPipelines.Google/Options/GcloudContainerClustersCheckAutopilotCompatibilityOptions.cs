using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "clusters", "check-autopilot-compatibility")]
public record GcloudContainerClustersCheckAutopilotCompatibilityOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}