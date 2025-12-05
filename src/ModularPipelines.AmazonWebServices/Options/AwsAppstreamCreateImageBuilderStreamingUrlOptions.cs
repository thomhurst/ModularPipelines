using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-image-builder-streaming-url")]
public record AwsAppstreamCreateImageBuilderStreamingUrlOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--validity")]
    public long? Validity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}