using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "create-storage-virtual-machine")]
public record AwsFsxCreateStorageVirtualMachineOptions(
[property: CliOption("--file-system-id")] string FileSystemId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--active-directory-configuration")]
    public string? ActiveDirectoryConfiguration { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--svm-admin-password")]
    public string? SvmAdminPassword { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--root-volume-security-style")]
    public string? RootVolumeSecurityStyle { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}