using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs", "create-channel")]
public record AwsIvsCreateChannelOptions : AwsOptions
{
    [CliOption("--latency-mode")]
    public string? LatencyMode { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--preset")]
    public string? Preset { get; set; }

    [CliOption("--recording-configuration-arn")]
    public string? RecordingConfigurationArn { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}