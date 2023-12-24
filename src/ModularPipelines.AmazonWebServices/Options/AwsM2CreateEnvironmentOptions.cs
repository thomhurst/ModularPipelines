using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("m2", "create-environment")]
public record AwsM2CreateEnvironmentOptions(
[property: CommandSwitch("--engine-type")] string EngineType,
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--high-availability-config")]
    public string? HighAvailabilityConfig { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--storage-configurations")]
    public string[]? StorageConfigurations { get; set; }

    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}