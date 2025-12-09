using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastictranscoder", "create-pipeline")]
public record AwsElastictranscoderCreatePipelineOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--input-bucket")] string InputBucket,
[property: CliOption("--role")] string Role
) : AwsOptions
{
    [CliOption("--output-bucket")]
    public string? OutputBucket { get; set; }

    [CliOption("--aws-kms-key-arn")]
    public string? AwsKmsKeyArn { get; set; }

    [CliOption("--notifications")]
    public string? Notifications { get; set; }

    [CliOption("--content-config")]
    public string? ContentConfig { get; set; }

    [CliOption("--thumbnail-config")]
    public string? ThumbnailConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}