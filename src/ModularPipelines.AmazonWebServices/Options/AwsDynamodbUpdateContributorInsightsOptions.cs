using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "update-contributor-insights")]
public record AwsDynamodbUpdateContributorInsightsOptions(
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--contributor-insights-action")] string ContributorInsightsAction
) : AwsOptions
{
    [CliOption("--index-name")]
    public string? IndexName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}