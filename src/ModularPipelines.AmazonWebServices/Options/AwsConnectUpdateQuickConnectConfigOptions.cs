using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-quick-connect-config")]
public record AwsConnectUpdateQuickConnectConfigOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--quick-connect-id")] string QuickConnectId,
[property: CommandSwitch("--quick-connect-config")] string QuickConnectConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}