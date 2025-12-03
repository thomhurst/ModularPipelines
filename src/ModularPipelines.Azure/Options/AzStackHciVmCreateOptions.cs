using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stack-hci-vm", "create")]
public record AzStackHciVmCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliFlag("--attach-data-disks")]
    public bool? AttachDataDisks { get; set; }

    [CliOption("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CliOption("--computer-name")]
    public string? ComputerName { get; set; }

    [CliFlag("--enable-agent")]
    public bool? EnableAgent { get; set; }

    [CliFlag("--enable-secure-boot")]
    public bool? EnableSecureBoot { get; set; }

    [CliFlag("--enable-vm-config-agent")]
    public bool? EnableVmConfigAgent { get; set; }

    [CliFlag("--enable-vtpm")]
    public bool? EnableVtpm { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--nics")]
    public string? Nics { get; set; }

    [CliOption("--os-disk-name")]
    public string? OsDiskName { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CliOption("--security-type")]
    public string? SecurityType { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }

    [CliOption("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CliOption("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CliOption("--storage-path-id")]
    public string? StoragePathId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}