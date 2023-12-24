using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "describe-contributor-insights")]
public record AwsDynamodbDescribeContributorInsightsOptions(
[property: CommandSwitch("--table-name")] string TableName
) : AwsOptions
{
    [CommandSwitch("--index-name")]
    public string? IndexName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}