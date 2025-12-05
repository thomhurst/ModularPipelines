using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "batch-disassociate-analytics-data-set")]
public record AwsConnectBatchDisassociateAnalyticsDataSetOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--data-set-ids")] string[] DataSetIds
) : AwsOptions
{
    [CliOption("--target-account-id")]
    public string? TargetAccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}