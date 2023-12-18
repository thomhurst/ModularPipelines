using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("advisor", "recommendation", "list")]
public record AzAdvisorRecommendationListOptions : AzOptions
{
    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--refresh")]
    public string? Refresh { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}