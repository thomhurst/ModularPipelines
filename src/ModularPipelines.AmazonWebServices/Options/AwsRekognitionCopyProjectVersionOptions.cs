using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "copy-project-version")]
public record AwsRekognitionCopyProjectVersionOptions(
[property: CliOption("--source-project-arn")] string SourceProjectArn,
[property: CliOption("--source-project-version-arn")] string SourceProjectVersionArn,
[property: CliOption("--destination-project-arn")] string DestinationProjectArn,
[property: CliOption("--version-name")] string VersionName,
[property: CliOption("--output-config")] string OutputConfig
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}