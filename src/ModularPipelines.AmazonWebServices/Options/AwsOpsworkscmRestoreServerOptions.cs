using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworkscm", "restore-server")]
public record AwsOpsworkscmRestoreServerOptions(
[property: CommandSwitch("--backup-id")] string BackupId,
[property: CommandSwitch("--server-name")] string ServerName
) : AwsOptions
{
    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--key-pair")]
    public string? KeyPair { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}