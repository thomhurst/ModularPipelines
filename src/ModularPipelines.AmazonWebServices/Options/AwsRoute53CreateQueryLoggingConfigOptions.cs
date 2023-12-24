using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "create-query-logging-config")]
public record AwsRoute53CreateQueryLoggingConfigOptions(
[property: CommandSwitch("--hosted-zone-id")] string HostedZoneId,
[property: CommandSwitch("--cloud-watch-logs-log-group-arn")] string CloudWatchLogsLogGroupArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}