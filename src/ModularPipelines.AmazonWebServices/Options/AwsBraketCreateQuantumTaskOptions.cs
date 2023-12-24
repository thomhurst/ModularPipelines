using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("braket", "create-quantum-task")]
public record AwsBraketCreateQuantumTaskOptions(
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--device-arn")] string DeviceArn,
[property: CommandSwitch("--output-s3-bucket")] string OutputS3Bucket,
[property: CommandSwitch("--output-s3-key-prefix")] string OutputS3KeyPrefix,
[property: CommandSwitch("--shots")] long Shots
) : AwsOptions
{
    [CommandSwitch("--associations")]
    public string[]? Associations { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--device-parameters")]
    public string? DeviceParameters { get; set; }

    [CommandSwitch("--job-token")]
    public string? JobToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}