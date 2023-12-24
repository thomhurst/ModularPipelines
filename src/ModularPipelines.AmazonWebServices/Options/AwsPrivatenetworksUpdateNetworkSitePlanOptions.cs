using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privatenetworks", "update-network-site-plan")]
public record AwsPrivatenetworksUpdateNetworkSitePlanOptions(
[property: CommandSwitch("--network-site-arn")] string NetworkSiteArn,
[property: CommandSwitch("--pending-plan")] string PendingPlan
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}