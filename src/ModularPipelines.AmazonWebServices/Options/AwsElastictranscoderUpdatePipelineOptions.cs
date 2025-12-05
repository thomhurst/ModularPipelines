using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastictranscoder", "update-pipeline")]
public record AwsElastictranscoderUpdatePipelineOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--input-bucket")]
    public string? InputBucket { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

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