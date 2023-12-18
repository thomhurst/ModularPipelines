using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "virtualmachine", "create")]
public record AzNetworkcloudVirtualmachineCreateOptions(
[property: CommandSwitch("--admin-username")] string AdminUsername,
[property: CommandSwitch("--cloud-services-network-attachment")] string CloudServicesNetworkAttachment,
[property: CommandSwitch("--cpu-cores")] string CpuCores,
[property: CommandSwitch("--extended-location")] string ExtendedLocation,
[property: CommandSwitch("--memory-size")] string MemorySize,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-profile")] string StorageProfile,
[property: CommandSwitch("--vm-image")] string VmImage
) : AzOptions
{
    [CommandSwitch("--boot-method")]
    public string? BootMethod { get; set; }

    [BooleanCommandSwitch("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--na")]
    public string? Na { get; set; }

    [CommandSwitch("--nd")]
    public string? Nd { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--ph")]
    public string? Ph { get; set; }

    [CommandSwitch("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CommandSwitch("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--ud")]
    public string? Ud { get; set; }

    [CommandSwitch("--vm-device-model")]
    public string? VmDeviceModel { get; set; }

    [CommandSwitch("--vm-image-repository-credentials")]
    public string? VmImageRepositoryCredentials { get; set; }
}