using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-realtime-log-config")]
public record AwsCloudfrontCreateRealtimeLogConfigOptions(
[property: CliOption("--end-points")] string[] EndPoints,
[property: CliOption("--fields")] string[] Fields,
[property: CliOption("--name")] string Name,
[property: CliOption("--sampling-rate")] long SamplingRate
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}