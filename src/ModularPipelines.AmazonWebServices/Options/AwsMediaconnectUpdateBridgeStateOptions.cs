using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "update-bridge-state")]
public record AwsMediaconnectUpdateBridgeStateOptions(
[property: CommandSwitch("--bridge-arn")] string BridgeArn,
[property: CommandSwitch("--desired-state")] string DesiredState
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}