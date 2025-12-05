using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-geospatial", "start-earth-observation-job")]
public record AwsSagemakerGeospatialStartEarthObservationJobOptions(
[property: CliOption("--execution-role-arn")] string ExecutionRoleArn,
[property: CliOption("--input-config")] string InputConfig,
[property: CliOption("--job-config")] string JobConfig,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}