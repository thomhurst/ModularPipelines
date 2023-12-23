using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra-ranking", "rescore")]
public record AwsKendraRankingRescoreOptions(
[property: CommandSwitch("--rescore-execution-plan-id")] string RescoreExecutionPlanId,
[property: CommandSwitch("--search-query")] string SearchQuery,
[property: CommandSwitch("--documents")] string[] Documents
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}