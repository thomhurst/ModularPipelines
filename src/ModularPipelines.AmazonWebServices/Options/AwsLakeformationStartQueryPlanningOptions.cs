using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "start-query-planning")]
public record AwsLakeformationStartQueryPlanningOptions(
[property: CliOption("--query-planning-context")] string QueryPlanningContext,
[property: CliOption("--query-string")] string QueryString
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}