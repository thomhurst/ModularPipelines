using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "describe-view")]
public record AwsConnectDescribeViewOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--view-id")] string ViewId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}