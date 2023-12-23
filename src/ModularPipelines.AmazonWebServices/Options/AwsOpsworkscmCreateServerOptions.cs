using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworkscm", "create-server")]
public record AwsOpsworkscmCreateServerOptions(
[property: CommandSwitch("--engine")] string Engine,
[property: CommandSwitch("--server-name")] string ServerName,
[property: CommandSwitch("--instance-profile-arn")] string InstanceProfileArn,
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--service-role-arn")] string ServiceRoleArn
) : AwsOptions
{
    [CommandSwitch("--custom-domain")]
    public string? CustomDomain { get; set; }

    [CommandSwitch("--custom-certificate")]
    public string? CustomCertificate { get; set; }

    [CommandSwitch("--custom-private-key")]
    public string? CustomPrivateKey { get; set; }

    [CommandSwitch("--engine-model")]
    public string? EngineModel { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--engine-attributes")]
    public string[]? EngineAttributes { get; set; }

    [CommandSwitch("--backup-retention-count")]
    public int? BackupRetentionCount { get; set; }

    [CommandSwitch("--key-pair")]
    public string? KeyPair { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--backup-id")]
    public string? BackupId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}