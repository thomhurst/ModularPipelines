using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "create-file-system")]
public record AwsFsxCreateFileSystemOptions(
[property: CliOption("--file-system-type")] string FileSystemType,
[property: CliOption("--storage-capacity")] int StorageCapacity,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--windows-configuration")]
    public string? WindowsConfiguration { get; set; }

    [CliOption("--lustre-configuration")]
    public string? LustreConfiguration { get; set; }

    [CliOption("--ontap-configuration")]
    public string? OntapConfiguration { get; set; }

    [CliOption("--file-system-type-version")]
    public string? FileSystemTypeVersion { get; set; }

    [CliOption("--open-zfs-configuration")]
    public string? OpenZfsConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}