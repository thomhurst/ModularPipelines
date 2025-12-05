using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworkscm", "create-server")]
public record AwsOpsworkscmCreateServerOptions(
[property: CliOption("--engine")] string Engine,
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--instance-profile-arn")] string InstanceProfileArn,
[property: CliOption("--instance-type")] string InstanceType,
[property: CliOption("--service-role-arn")] string ServiceRoleArn
) : AwsOptions
{
    [CliOption("--custom-domain")]
    public string? CustomDomain { get; set; }

    [CliOption("--custom-certificate")]
    public string? CustomCertificate { get; set; }

    [CliOption("--custom-private-key")]
    public string? CustomPrivateKey { get; set; }

    [CliOption("--engine-model")]
    public string? EngineModel { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--engine-attributes")]
    public string[]? EngineAttributes { get; set; }

    [CliOption("--backup-retention-count")]
    public int? BackupRetentionCount { get; set; }

    [CliOption("--key-pair")]
    public string? KeyPair { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--preferred-backup-window")]
    public string? PreferredBackupWindow { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--backup-id")]
    public string? BackupId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}