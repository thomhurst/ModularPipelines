using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "batch-get-incident-findings")]
public record AwsSsmIncidentsBatchGetIncidentFindingsOptions(
[property: CliOption("--finding-ids")] string[] FindingIds,
[property: CliOption("--incident-record-arn")] string IncidentRecordArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}