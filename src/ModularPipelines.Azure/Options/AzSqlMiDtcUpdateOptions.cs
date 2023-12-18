using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi", "dtc", "update")]
public record AzSqlMiDtcUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--allow-inbound-enabled")]
    public bool? AllowInboundEnabled { get; set; }

    [BooleanCommandSwitch("--allow-outbound-enabled")]
    public bool? AllowOutboundEnabled { get; set; }

    [CommandSwitch("--authentication")]
    public string? Authentication { get; set; }

    [BooleanCommandSwitch("--dtc-enabled")]
    public bool? DtcEnabled { get; set; }

    [CommandSwitch("--external-dns-suffix-search-list")]
    public string? ExternalDnsSuffixSearchList { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--managed-instance-name")]
    public string? ManagedInstanceName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [BooleanCommandSwitch("--sna-lu-transactions")]
    public bool? SnaLuTransactions { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--xa-default-timeout")]
    public string? XaDefaultTimeout { get; set; }

    [CommandSwitch("--xa-max-timeout")]
    public string? XaMaxTimeout { get; set; }

    [BooleanCommandSwitch("--xa-transactions")]
    public bool? XaTransactions { get; set; }
}