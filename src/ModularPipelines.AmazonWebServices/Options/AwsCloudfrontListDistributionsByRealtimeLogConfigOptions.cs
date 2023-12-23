using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "list-distributions-by-realtime-log-config")]
public record AwsCloudfrontListDistributionsByRealtimeLogConfigOptions : AwsOptions
{
    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--realtime-log-config-name")]
    public string? RealtimeLogConfigName { get; set; }

    [CommandSwitch("--realtime-log-config-arn")]
    public string? RealtimeLogConfigArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}