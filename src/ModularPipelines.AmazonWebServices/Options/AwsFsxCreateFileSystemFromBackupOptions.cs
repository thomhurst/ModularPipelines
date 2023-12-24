using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "create-file-system-from-backup")]
public record AwsFsxCreateFileSystemFromBackupOptions(
[property: CommandSwitch("--backup-id")] string BackupId,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--windows-configuration")]
    public string? WindowsConfiguration { get; set; }

    [CommandSwitch("--lustre-configuration")]
    public string? LustreConfiguration { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--file-system-type-version")]
    public string? FileSystemTypeVersion { get; set; }

    [CommandSwitch("--open-zfs-configuration")]
    public string? OpenZfsConfiguration { get; set; }

    [CommandSwitch("--storage-capacity")]
    public int? StorageCapacity { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}