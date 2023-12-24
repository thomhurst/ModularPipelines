using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "grant-flow-entitlements")]
public record AwsMediaconnectGrantFlowEntitlementsOptions(
[property: CommandSwitch("--entitlements")] string[] Entitlements,
[property: CommandSwitch("--flow-arn")] string FlowArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}