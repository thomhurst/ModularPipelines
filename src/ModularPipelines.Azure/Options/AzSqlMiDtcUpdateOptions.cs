using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi", "dtc", "update")]
public record AzSqlMiDtcUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--allow-inbound-enabled")]
    public bool? AllowInboundEnabled { get; set; }

    [CliFlag("--allow-outbound-enabled")]
    public bool? AllowOutboundEnabled { get; set; }

    [CliOption("--authentication")]
    public string? Authentication { get; set; }

    [CliFlag("--dtc-enabled")]
    public bool? DtcEnabled { get; set; }

    [CliOption("--external-dns-suffix-search-list")]
    public string? ExternalDnsSuffixSearchList { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--managed-instance-name")]
    public string? ManagedInstanceName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--sna-lu-transactions")]
    public bool? SnaLuTransactions { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--xa-default-timeout")]
    public string? XaDefaultTimeout { get; set; }

    [CliOption("--xa-max-timeout")]
    public string? XaMaxTimeout { get; set; }

    [CliFlag("--xa-transactions")]
    public bool? XaTransactions { get; set; }
}