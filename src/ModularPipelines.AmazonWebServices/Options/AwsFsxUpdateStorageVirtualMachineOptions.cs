using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "update-storage-virtual-machine")]
public record AwsFsxUpdateStorageVirtualMachineOptions(
[property: CommandSwitch("--storage-virtual-machine-id")] string StorageVirtualMachineId
) : AwsOptions
{
    [CommandSwitch("--active-directory-configuration")]
    public string? ActiveDirectoryConfiguration { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--svm-admin-password")]
    public string? SvmAdminPassword { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}