using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "update-function-code")]
public record AwsLambdaUpdateFunctionCodeOptions(
[property: CliOption("--function-name")] string FunctionName
) : AwsOptions
{
    [CliOption("--zip-file")]
    public string? ZipFile { get; set; }

    [CliOption("--s3-bucket")]
    public string? S3Bucket { get; set; }

    [CliOption("--s3-key")]
    public string? S3Key { get; set; }

    [CliOption("--s3-object-version")]
    public string? S3ObjectVersion { get; set; }

    [CliOption("--image-uri")]
    public string? ImageUri { get; set; }

    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--architectures")]
    public string[]? Architectures { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}