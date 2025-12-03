using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "sim", "policy", "create")]
public record AzMobileNetworkSimPolicyCreateOptions(
[property: CliOption("--default-slice")] string DefaultSlice,
[property: CliOption("--mobile-network-name")] string MobileNetworkName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--slice-config")] string SliceConfig,
[property: CliOption("--ue-ambr")] string UeAmbr
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--registration-timer")]
    public string? RegistrationTimer { get; set; }

    [CliOption("--rfsp-index")]
    public string? RfspIndex { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}