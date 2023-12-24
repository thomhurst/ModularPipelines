using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "batch-associate-analytics-data-set")]
public record AwsConnectBatchAssociateAnalyticsDataSetOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--data-set-ids")] string[] DataSetIds
) : AwsOptions
{
    [CommandSwitch("--target-account-id")]
    public string? TargetAccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}