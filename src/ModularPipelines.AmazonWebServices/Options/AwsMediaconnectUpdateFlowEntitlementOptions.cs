using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "update-flow-entitlement")]
public record AwsMediaconnectUpdateFlowEntitlementOptions(
[property: CommandSwitch("--entitlement-arn")] string EntitlementArn,
[property: CommandSwitch("--flow-arn")] string FlowArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--encryption")]
    public string? Encryption { get; set; }

    [CommandSwitch("--entitlement-status")]
    public string? EntitlementStatus { get; set; }

    [CommandSwitch("--subscribers")]
    public string[]? Subscribers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}