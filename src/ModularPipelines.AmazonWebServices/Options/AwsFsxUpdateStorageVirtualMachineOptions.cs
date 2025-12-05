using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "update-storage-virtual-machine")]
public record AwsFsxUpdateStorageVirtualMachineOptions(
[property: CliOption("--storage-virtual-machine-id")] string StorageVirtualMachineId
) : AwsOptions
{
    [CliOption("--active-directory-configuration")]
    public string? ActiveDirectoryConfiguration { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--svm-admin-password")]
    public string? SvmAdminPassword { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}