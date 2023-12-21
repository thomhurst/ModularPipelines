using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "classification", "recommendation", "list")]
public record AzSqlDbClassificationRecommendationListOptions : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--include-disabled-recommendations")]
    public bool? IncludeDisabledRecommendations { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}