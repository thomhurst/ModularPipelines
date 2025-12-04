using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "midb", "short-term-retention-policy", "set")]
public record AzSqlMidbShortTermRetentionPolicySetOptions(
[property: CliOption("--retention-days")] string RetentionDays
) : AzOptions
{
    [CliOption("--deleted-time")]
    public string? DeletedTime { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}