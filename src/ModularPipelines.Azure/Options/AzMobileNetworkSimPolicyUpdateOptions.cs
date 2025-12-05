using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network", "sim", "policy", "update")]
public record AzMobileNetworkSimPolicyUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--default-slice")]
    public string? DefaultSlice { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--mobile-network-name")]
    public string? MobileNetworkName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--registration-timer")]
    public string? RegistrationTimer { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rfsp-index")]
    public string? RfspIndex { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--slice-config")]
    public string? SliceConfig { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--ue-ambr")]
    public string? UeAmbr { get; set; }
}