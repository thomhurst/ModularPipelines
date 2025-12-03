using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "update-realtime-log-config")]
public record AwsCloudfrontUpdateRealtimeLogConfigOptions : AwsOptions
{
    [CliOption("--end-points")]
    public string[]? EndPoints { get; set; }

    [CliOption("--fields")]
    public string[]? Fields { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--arn")]
    public string? Arn { get; set; }

    [CliOption("--sampling-rate")]
    public long? SamplingRate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}