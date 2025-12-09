using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis", "create-stream")]
public record AwsKinesisCreateStreamOptions(
[property: CliOption("--stream-name")] string StreamName
) : AwsOptions
{
    [CliOption("--shard-count")]
    public int? ShardCount { get; set; }

    [CliOption("--stream-mode-details")]
    public string? StreamModeDetails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}