using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("detective", "batch-get-graph-member-datasources")]
public record AwsDetectiveBatchGetGraphMemberDatasourcesOptions(
[property: CommandSwitch("--graph-arn")] string GraphArn,
[property: CommandSwitch("--account-ids")] string[] AccountIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}