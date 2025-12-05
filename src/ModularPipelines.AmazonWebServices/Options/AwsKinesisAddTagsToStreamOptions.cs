using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis", "add-tags-to-stream")]
public record AwsKinesisAddTagsToStreamOptions(
[property: CliOption("--tags")] IEnumerable<KeyValue> Tags
) : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}