using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "failover-group", "set-primary")]
public record AzSqlFailoverGroupSetPrimaryOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--allow-data-loss")]
    public bool? AllowDataLoss { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--tpbff")]
    public bool? Tpbff { get; set; }
}