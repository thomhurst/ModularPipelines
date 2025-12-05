using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute-optimizer", "get-ecs-service-recommendations")]
public record AwsComputeOptimizerGetEcsServiceRecommendationsOptions : AwsOptions
{
    [CliOption("--service-arns")]
    public string[]? ServiceArns { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}