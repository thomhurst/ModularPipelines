using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgh", "delete-progress-update-stream")]
public record AwsMghDeleteProgressUpdateStreamOptions(
[property: CliOption("--progress-update-stream-name")] string ProgressUpdateStreamName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}