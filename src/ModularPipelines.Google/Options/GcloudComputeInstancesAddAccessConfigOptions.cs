using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "add-access-config")]
public record GcloudComputeInstancesAddAccessConfigOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--access-config-name")]
    public string? AccessConfigName { get; set; }

    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--network-interface")]
    public string? NetworkInterface { get; set; }

    [CliOption("--network-tier")]
    public string? NetworkTier { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliFlag("--public-ptr")]
    public bool? PublicPtr { get; set; }

    [CliFlag("--no-public-ptr")]
    public bool? NoPublicPtr { get; set; }

    [CliOption("--public-ptr-domain")]
    public string? PublicPtrDomain { get; set; }

    [CliFlag("--no-public-ptr-domain")]
    public bool? NoPublicPtrDomain { get; set; }
}