using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "db", "classification", "list")]
public record AzSqlDbClassificationListOptions : AzOptions
{
    [CliOption("--count")]
    public int? Count { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}