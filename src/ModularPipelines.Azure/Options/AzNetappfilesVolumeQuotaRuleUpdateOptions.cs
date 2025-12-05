using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "volume", "quota-rule", "update")]
public record AzNetappfilesVolumeQuotaRuleUpdateOptions(
[property: CliOption("--quota-rule-name")] string QuotaRuleName
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pool-name")]
    public string? PoolName { get; set; }

    [CliOption("--quota-size")]
    public string? QuotaSize { get; set; }

    [CliOption("--quota-target")]
    public string? QuotaTarget { get; set; }

    [CliOption("--quota-type")]
    public string? QuotaType { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--volume-name")]
    public string? VolumeName { get; set; }
}