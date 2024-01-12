using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudRecommender
{
    public GcloudRecommender(
        GcloudRecommenderInsightTypeConfig insightTypeConfig,
        GcloudRecommenderInsights insights,
        GcloudRecommenderRecommendations recommendations,
        GcloudRecommenderRecommenderConfig recommenderConfig
    )
    {
        InsightTypeConfig = insightTypeConfig;
        Insights = insights;
        Recommendations = recommendations;
        RecommenderConfig = recommenderConfig;
    }

    public GcloudRecommenderInsightTypeConfig InsightTypeConfig { get; }

    public GcloudRecommenderInsights Insights { get; }

    public GcloudRecommenderRecommendations Recommendations { get; }

    public GcloudRecommenderRecommenderConfig RecommenderConfig { get; }
}