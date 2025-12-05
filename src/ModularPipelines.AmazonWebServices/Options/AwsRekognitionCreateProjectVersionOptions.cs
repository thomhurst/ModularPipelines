using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "create-project-version")]
public record AwsRekognitionCreateProjectVersionOptions(
[property: CliOption("--project-arn")] string ProjectArn,
[property: CliOption("--version-name")] string VersionName,
[property: CliOption("--output-config")] string OutputConfig
) : AwsOptions
{
    [CliOption("--training-data")]
    public string? TrainingData { get; set; }

    [CliOption("--testing-data")]
    public string? TestingData { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliOption("--feature-config")]
    public string? FeatureConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}