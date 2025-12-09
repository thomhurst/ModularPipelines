using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-rightsizing-recommendation")]
public record AwsCeGetRightsizingRecommendationOptions(
[property: CliOption("--service")] string Service
) : AwsOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}