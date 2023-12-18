using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAdvisor
{
    public AzAdvisor(
        AzAdvisorConfiguration configuration,
        AzAdvisorRecommendation recommendation
    )
    {
        Configuration = configuration;
        Recommendation = recommendation;
    }

    public AzAdvisorConfiguration Configuration { get; }

    public AzAdvisorRecommendation Recommendation { get; }
}