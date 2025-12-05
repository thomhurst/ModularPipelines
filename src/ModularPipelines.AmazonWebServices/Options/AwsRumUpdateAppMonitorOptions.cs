using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rum", "update-app-monitor")]
public record AwsRumUpdateAppMonitorOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--app-monitor-configuration")]
    public string? AppMonitorConfiguration { get; set; }

    [CliOption("--custom-events")]
    public string? CustomEvents { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}