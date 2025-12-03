using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("image", "builder", "create")]
public record AzImageBuilderCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--build-timeout")]
    public string? BuildTimeout { get; set; }

    [CliOption("--build-vm-identities")]
    public string? BuildVmIdentities { get; set; }

    [CliOption("--checksum")]
    public string? Checksum { get; set; }

    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--image-source")]
    public string? ImageSource { get; set; }

    [CliOption("--image-template")]
    public string? ImageTemplate { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-image-destinations")]
    public string? ManagedImageDestinations { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--os-disk-size")]
    public string? OsDiskSize { get; set; }

    [CliOption("--proxy-vm-size")]
    public string? ProxyVmSize { get; set; }

    [CliOption("--scripts")]
    public string? Scripts { get; set; }

    [CliOption("--shared-image-destinations")]
    public string? SharedImageDestinations { get; set; }

    [CliOption("--staging-resource-group")]
    public string? StagingResourceGroup { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--validator")]
    public string? Validator { get; set; }

    [CliOption("--vm-size")]
    public string? VmSize { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }
}