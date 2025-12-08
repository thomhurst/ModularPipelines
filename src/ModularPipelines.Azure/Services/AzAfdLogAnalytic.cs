using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd")]
public class AzAfdLogAnalytic
{
    public AzAfdLogAnalytic(
        AzAfdLogAnalyticLocation location,
        AzAfdLogAnalyticMetric metric,
        AzAfdLogAnalyticRanking ranking,
        AzAfdLogAnalyticResource resource
    )
    {
        Location = location;
        Metric = metric;
        Ranking = ranking;
        Resource = resource;
    }

    public AzAfdLogAnalyticLocation Location { get; }

    public AzAfdLogAnalyticMetric Metric { get; }

    public AzAfdLogAnalyticRanking Ranking { get; }

    public AzAfdLogAnalyticResource Resource { get; }
}