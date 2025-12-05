using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackage-vod", "configure-logs")]
public record AwsMediapackageVodConfigureLogsOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--egress-access-logs")]
    public string? EgressAccessLogs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}