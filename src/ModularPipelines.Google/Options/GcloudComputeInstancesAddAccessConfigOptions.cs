using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "add-access-config")]
public record GcloudComputeInstancesAddAccessConfigOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--access-config-name")]
    public string? AccessConfigName { get; set; }

    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--network-interface")]
    public string? NetworkInterface { get; set; }

    [CommandSwitch("--network-tier")]
    public string? NetworkTier { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [BooleanCommandSwitch("--public-ptr")]
    public bool? PublicPtr { get; set; }

    [BooleanCommandSwitch("--no-public-ptr")]
    public bool? NoPublicPtr { get; set; }

    [CommandSwitch("--public-ptr-domain")]
    public string? PublicPtrDomain { get; set; }

    [BooleanCommandSwitch("--no-public-ptr-domain")]
    public bool? NoPublicPtrDomain { get; set; }
}