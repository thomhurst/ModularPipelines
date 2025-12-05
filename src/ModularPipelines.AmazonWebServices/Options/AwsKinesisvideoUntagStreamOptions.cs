using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisvideo", "untag-stream")]
public record AwsKinesisvideoUntagStreamOptions(
[property: CliOption("--tag-key-list")] string[] TagKeyList
) : AwsOptions
{
    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}