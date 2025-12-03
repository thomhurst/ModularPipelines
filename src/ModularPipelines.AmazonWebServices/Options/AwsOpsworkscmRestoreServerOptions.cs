using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworkscm", "restore-server")]
public record AwsOpsworkscmRestoreServerOptions(
[property: CliOption("--backup-id")] string BackupId,
[property: CliOption("--server-name")] string ServerName
) : AwsOptions
{
    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--key-pair")]
    public string? KeyPair { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}