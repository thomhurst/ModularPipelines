using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "add-bridge-outputs")]
public record AwsMediaconnectAddBridgeOutputsOptions(
[property: CommandSwitch("--bridge-arn")] string BridgeArn,
[property: CommandSwitch("--outputs")] string[] Outputs
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}