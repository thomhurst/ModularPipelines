using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "create-storage-virtual-machine")]
public record AwsFsxCreateStorageVirtualMachineOptions(
[property: CommandSwitch("--file-system-id")] string FileSystemId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--active-directory-configuration")]
    public string? ActiveDirectoryConfiguration { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--svm-admin-password")]
    public string? SvmAdminPassword { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--root-volume-security-style")]
    public string? RootVolumeSecurityStyle { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}