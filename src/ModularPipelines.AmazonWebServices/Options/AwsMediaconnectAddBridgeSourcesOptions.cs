using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "add-bridge-sources")]
public record AwsMediaconnectAddBridgeSourcesOptions(
[property: CommandSwitch("--bridge-arn")] string BridgeArn,
[property: CommandSwitch("--sources")] string[] Sources
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}