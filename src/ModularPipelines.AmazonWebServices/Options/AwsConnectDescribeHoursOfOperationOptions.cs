using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "describe-hours-of-operation")]
public record AwsConnectDescribeHoursOfOperationOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--hours-of-operation-id")] string HoursOfOperationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}