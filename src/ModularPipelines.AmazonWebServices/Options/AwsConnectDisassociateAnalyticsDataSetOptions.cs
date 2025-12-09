using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "disassociate-analytics-data-set")]
public record AwsConnectDisassociateAnalyticsDataSetOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--data-set-id")] string DataSetId
) : AwsOptions
{
    [CliOption("--target-account-id")]
    public string? TargetAccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}