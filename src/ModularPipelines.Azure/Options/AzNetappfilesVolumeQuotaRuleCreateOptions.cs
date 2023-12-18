using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume", "quota-rule", "create")]
public record AzNetappfilesVolumeQuotaRuleCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--quota-rule-name")] string QuotaRuleName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--volume-name")] string VolumeName
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--quota-size")]
    public string? QuotaSize { get; set; }

    [CommandSwitch("--quota-target")]
    public string? QuotaTarget { get; set; }

    [CommandSwitch("--quota-type")]
    public string? QuotaType { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

