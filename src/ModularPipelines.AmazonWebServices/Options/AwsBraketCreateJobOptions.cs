using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("braket", "create-job")]
public record AwsBraketCreateJobOptions(
[property: CliOption("--algorithm-specification")] string AlgorithmSpecification,
[property: CliOption("--device-config")] string DeviceConfig,
[property: CliOption("--instance-config")] string InstanceConfig,
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--output-data-config")] string OutputDataConfig,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--associations")]
    public string[]? Associations { get; set; }

    [CliOption("--checkpoint-config")]
    public string? CheckpointConfig { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--hyper-parameters")]
    public IEnumerable<KeyValue>? HyperParameters { get; set; }

    [CliOption("--input-data-config")]
    public string[]? InputDataConfig { get; set; }

    [CliOption("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}