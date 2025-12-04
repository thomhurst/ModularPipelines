using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "log-profiles", "update")]
public record AzMonitorLogProfilesUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--categories")]
    public string? Categories { get; set; }

    [CliOption("--days")]
    public int? Days { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--locations")]
    public string? Locations { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--service-bus-rule-id")]
    public string? ServiceBusRuleId { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--storage-account-id")]
    public int? StorageAccountId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}