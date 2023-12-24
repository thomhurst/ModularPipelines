using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-incidents", "batch-get-incident-findings")]
public record AwsSsmIncidentsBatchGetIncidentFindingsOptions(
[property: CommandSwitch("--finding-ids")] string[] FindingIds,
[property: CommandSwitch("--incident-record-arn")] string IncidentRecordArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}