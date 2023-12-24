using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "update-bridge-source")]
public record AwsMediaconnectUpdateBridgeSourceOptions(
[property: CommandSwitch("--bridge-arn")] string BridgeArn,
[property: CommandSwitch("--source-name")] string SourceName
) : AwsOptions
{
    [CommandSwitch("--flow-source")]
    public string? FlowSource { get; set; }

    [CommandSwitch("--network-source")]
    public string? NetworkSource { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}