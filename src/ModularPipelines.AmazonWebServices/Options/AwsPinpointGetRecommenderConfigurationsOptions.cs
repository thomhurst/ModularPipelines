using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-recommender-configurations")]
public record AwsPinpointGetRecommenderConfigurationsOptions : AwsOptions
{
    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}