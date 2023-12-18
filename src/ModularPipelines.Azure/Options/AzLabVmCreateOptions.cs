using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lab", "vm", "create")]
public record AzLabVmCreateOptions(
[property: CommandSwitch("--lab-name")] string LabName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--admin-username")]
    public string? AdminUsername { get; set; }

    [BooleanCommandSwitch("--allow-claim")]
    public bool? AllowClaim { get; set; }

    [CommandSwitch("--artifacts")]
    public string? Artifacts { get; set; }

    [CommandSwitch("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CommandSwitch("--disk-type")]
    public string? DiskType { get; set; }

    [CommandSwitch("--expiration-date")]
    public string? ExpirationDate { get; set; }

    [CommandSwitch("--formula")]
    public string? Formula { get; set; }

    [CommandSwitch("--generate-ssh-keys")]
    public string? GenerateSshKeys { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--image-type")]
    public string? ImageType { get; set; }

    [CommandSwitch("--ip-configuration")]
    public string? IpConfiguration { get; set; }

    [CommandSwitch("--notes")]
    public string? Notes { get; set; }

    [CommandSwitch("--saved-secret")]
    public string? SavedSecret { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [CommandSwitch("--ssh-key")]
    public string? SshKey { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}