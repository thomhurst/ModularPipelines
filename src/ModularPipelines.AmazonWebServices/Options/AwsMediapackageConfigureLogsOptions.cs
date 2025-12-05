using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackage", "configure-logs")]
public record AwsMediapackageConfigureLogsOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--egress-access-logs")]
    public string? EgressAccessLogs { get; set; }

    [CliOption("--ingress-access-logs")]
    public string? IngressAccessLogs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}