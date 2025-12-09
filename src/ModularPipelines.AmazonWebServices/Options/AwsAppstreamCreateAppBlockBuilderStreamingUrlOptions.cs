using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-app-block-builder-streaming-url")]
public record AwsAppstreamCreateAppBlockBuilderStreamingUrlOptions(
[property: CliOption("--app-block-builder-name")] string AppBlockBuilderName
) : AwsOptions
{
    [CliOption("--validity")]
    public long? Validity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}