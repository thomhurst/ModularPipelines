using System.Diagnostics.CodeAnalysis;

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