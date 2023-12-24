using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("braket", "create-job")]
public record AwsBraketCreateJobOptions(
[property: CommandSwitch("--algorithm-specification")] string AlgorithmSpecification,
[property: CommandSwitch("--device-config")] string DeviceConfig,
[property: CommandSwitch("--instance-config")] string InstanceConfig,
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--output-data-config")] string OutputDataConfig,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--associations")]
    public string[]? Associations { get; set; }

    [CommandSwitch("--checkpoint-config")]
    public string? CheckpointConfig { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--hyper-parameters")]
    public IEnumerable<KeyValue>? HyperParameters { get; set; }

    [CommandSwitch("--input-data-config")]
    public string[]? InputDataConfig { get; set; }

    [CommandSwitch("--stopping-condition")]
    public string? StoppingCondition { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}