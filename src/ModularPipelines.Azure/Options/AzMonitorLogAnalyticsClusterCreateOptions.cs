using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-analytics", "cluster", "create")]
public record AzMonitorLogAnalyticsClusterCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--billing-type")]
    public string? BillingType { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--key-rsa-size")]
    public string? KeyRsaSize { get; set; }

    [CliOption("--key-vault-uri")]
    public string? KeyVaultUri { get; set; }

    [CliOption("--key-version")]
    public string? KeyVersion { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sku-capacity")]
    public string? SkuCapacity { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-assigned")]
    public string? UserAssigned { get; set; }
}