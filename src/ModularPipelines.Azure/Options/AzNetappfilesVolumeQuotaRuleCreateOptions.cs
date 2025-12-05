using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "volume", "quota-rule", "create")]
public record AzNetappfilesVolumeQuotaRuleCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--quota-rule-name")] string QuotaRuleName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--volume-name")] string VolumeName
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--quota-size")]
    public string? QuotaSize { get; set; }

    [CliOption("--quota-target")]
    public string? QuotaTarget { get; set; }

    [CliOption("--quota-type")]
    public string? QuotaType { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}