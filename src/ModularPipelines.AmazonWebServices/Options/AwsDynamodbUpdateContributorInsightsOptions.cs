using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "update-contributor-insights")]
public record AwsDynamodbUpdateContributorInsightsOptions(
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--contributor-insights-action")] string ContributorInsightsAction
) : AwsOptions
{
    [CommandSwitch("--index-name")]
    public string? IndexName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}