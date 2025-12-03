using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "virtualmachine", "create")]
public record AzNetworkcloudVirtualmachineCreateOptions(
[property: CliOption("--admin-username")] string AdminUsername,
[property: CliOption("--cloud-services-network-attachment")] string CloudServicesNetworkAttachment,
[property: CliOption("--cpu-cores")] string CpuCores,
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--memory-size")] string MemorySize,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-profile")] string StorageProfile,
[property: CliOption("--vm-image")] string VmImage
) : AzOptions
{
    [CliOption("--boot-method")]
    public string? BootMethod { get; set; }

    [CliFlag("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--na")]
    public string? Na { get; set; }

    [CliOption("--nd")]
    public string? Nd { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--ph")]
    public string? Ph { get; set; }

    [CliOption("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CliOption("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--ud")]
    public string? Ud { get; set; }

    [CliOption("--vm-device-model")]
    public string? VmDeviceModel { get; set; }

    [CliOption("--vm-image-repository-credentials")]
    public string? VmImageRepositoryCredentials { get; set; }
}