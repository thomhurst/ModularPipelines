using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastictranscoder", "create-preset")]
public record AwsElastictranscoderCreatePresetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--container")] string Container
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--video")]
    public string? Video { get; set; }

    [CliOption("--audio")]
    public string? Audio { get; set; }

    [CliOption("--thumbnails")]
    public string? Thumbnails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}