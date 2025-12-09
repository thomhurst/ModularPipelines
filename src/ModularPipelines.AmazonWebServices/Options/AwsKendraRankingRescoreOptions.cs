using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra-ranking", "rescore")]
public record AwsKendraRankingRescoreOptions(
[property: CliOption("--rescore-execution-plan-id")] string RescoreExecutionPlanId,
[property: CliOption("--search-query")] string SearchQuery,
[property: CliOption("--documents")] string[] Documents
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}