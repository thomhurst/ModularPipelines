using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "create-query-logging-config")]
public record AwsRoute53CreateQueryLoggingConfigOptions(
[property: CliOption("--hosted-zone-id")] string HostedZoneId,
[property: CliOption("--cloud-watch-logs-log-group-arn")] string CloudWatchLogsLogGroupArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}