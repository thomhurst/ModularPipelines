using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privatenetworks", "update-network-site-plan")]
public record AwsPrivatenetworksUpdateNetworkSitePlanOptions(
[property: CliOption("--network-site-arn")] string NetworkSiteArn,
[property: CliOption("--pending-plan")] string PendingPlan
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}