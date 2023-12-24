using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker-geospatial", "start-earth-observation-job")]
public record AwsSagemakerGeospatialStartEarthObservationJobOptions(
[property: CommandSwitch("--execution-role-arn")] string ExecutionRoleArn,
[property: CommandSwitch("--input-config")] string InputConfig,
[property: CommandSwitch("--job-config")] string JobConfig,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}