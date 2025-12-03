using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lab", "vm", "create")]
public record AzLabVmCreateOptions(
[property: CliOption("--lab-name")] string LabName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliFlag("--allow-claim")]
    public bool? AllowClaim { get; set; }

    [CliOption("--artifacts")]
    public string? Artifacts { get; set; }

    [CliOption("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CliOption("--disk-type")]
    public string? DiskType { get; set; }

    [CliOption("--expiration-date")]
    public string? ExpirationDate { get; set; }

    [CliOption("--formula")]
    public string? Formula { get; set; }

    [CliOption("--generate-ssh-keys")]
    public string? GenerateSshKeys { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--image-type")]
    public string? ImageType { get; set; }

    [CliOption("--ip-configuration")]
    public string? IpConfiguration { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--saved-secret")]
    public string? SavedSecret { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }

    [CliOption("--ssh-key")]
    public string? SshKey { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}