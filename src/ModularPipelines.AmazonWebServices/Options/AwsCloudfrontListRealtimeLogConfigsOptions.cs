using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "list-realtime-log-configs")]
public record AwsCloudfrontListRealtimeLogConfigsOptions : AwsOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}