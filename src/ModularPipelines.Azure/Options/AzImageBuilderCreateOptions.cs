using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("image", "builder", "create")]
public record AzImageBuilderCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--build-timeout")]
    public string? BuildTimeout { get; set; }

    [CommandSwitch("--build-vm-identities")]
    public string? BuildVmIdentities { get; set; }

    [CommandSwitch("--checksum")]
    public string? Checksum { get; set; }

    [CommandSwitch("--defer")]
    public string? Defer { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--image-source")]
    public string? ImageSource { get; set; }

    [CommandSwitch("--image-template")]
    public string? ImageTemplate { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-image-destinations")]
    public string? ManagedImageDestinations { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--os-disk-size")]
    public string? OsDiskSize { get; set; }

    [CommandSwitch("--proxy-vm-size")]
    public string? ProxyVmSize { get; set; }

    [CommandSwitch("--scripts")]
    public string? Scripts { get; set; }

    [CommandSwitch("--shared-image-destinations")]
    public string? SharedImageDestinations { get; set; }

    [CommandSwitch("--staging-resource-group")]
    public string? StagingResourceGroup { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--validator")]
    public string? Validator { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }
}