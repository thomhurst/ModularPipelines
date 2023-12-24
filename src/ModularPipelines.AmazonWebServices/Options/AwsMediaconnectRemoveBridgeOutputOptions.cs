using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "remove-bridge-output")]
public record AwsMediaconnectRemoveBridgeOutputOptions(
[property: CommandSwitch("--bridge-arn")] string BridgeArn,
[property: CommandSwitch("--output-name")] string OutputName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}