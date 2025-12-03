using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "list-distributions-by-realtime-log-config")]
public record AwsCloudfrontListDistributionsByRealtimeLogConfigOptions : AwsOptions
{
    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--realtime-log-config-name")]
    public string? RealtimeLogConfigName { get; set; }

    [CliOption("--realtime-log-config-arn")]
    public string? RealtimeLogConfigArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}