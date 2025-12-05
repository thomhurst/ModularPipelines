using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("braket", "create-quantum-task")]
public record AwsBraketCreateQuantumTaskOptions(
[property: CliOption("--action")] string Action,
[property: CliOption("--device-arn")] string DeviceArn,
[property: CliOption("--output-s3-bucket")] string OutputS3Bucket,
[property: CliOption("--output-s3-key-prefix")] string OutputS3KeyPrefix,
[property: CliOption("--shots")] long Shots
) : AwsOptions
{
    [CliOption("--associations")]
    public string[]? Associations { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--device-parameters")]
    public string? DeviceParameters { get; set; }

    [CliOption("--job-token")]
    public string? JobToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}