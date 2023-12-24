using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "create-file-system")]
public record AwsFsxCreateFileSystemOptions(
[property: CommandSwitch("--file-system-type")] string FileSystemType,
[property: CommandSwitch("--storage-capacity")] int StorageCapacity,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--windows-configuration")]
    public string? WindowsConfiguration { get; set; }

    [CommandSwitch("--lustre-configuration")]
    public string? LustreConfiguration { get; set; }

    [CommandSwitch("--ontap-configuration")]
    public string? OntapConfiguration { get; set; }

    [CommandSwitch("--file-system-type-version")]
    public string? FileSystemTypeVersion { get; set; }

    [CommandSwitch("--open-zfs-configuration")]
    public string? OpenZfsConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}