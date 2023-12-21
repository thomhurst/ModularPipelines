using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd")]
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