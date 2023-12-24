using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "disassociate-analytics-data-set")]
public record AwsConnectDisassociateAnalyticsDataSetOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--data-set-id")] string DataSetId
) : AwsOptions
{
    [CommandSwitch("--target-account-id")]
    public string? TargetAccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}