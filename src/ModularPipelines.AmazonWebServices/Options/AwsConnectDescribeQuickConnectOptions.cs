using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "describe-quick-connect")]
public record AwsConnectDescribeQuickConnectOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--quick-connect-id")] string QuickConnectId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}