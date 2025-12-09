using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd")]
public class AzAfdWafLogAnalytic
{
    public AzAfdWafLogAnalytic(
        AzAfdWafLogAnalyticMetric metric,
        AzAfdWafLogAnalyticRanking ranking
    )
    {
        Metric = metric;
        Ranking = ranking;
    }

    public AzAfdWafLogAnalyticMetric Metric { get; }

    public AzAfdWafLogAnalyticRanking Ranking { get; }
}