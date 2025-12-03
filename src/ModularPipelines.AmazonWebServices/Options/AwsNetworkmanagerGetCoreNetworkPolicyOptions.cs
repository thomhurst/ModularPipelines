using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "get-core-network-policy")]
public record AwsNetworkmanagerGetCoreNetworkPolicyOptions(
[property: CliOption("--core-network-id")] string CoreNetworkId
) : AwsOptions
{
    [CliOption("--policy-version-id")]
    public int? PolicyVersionId { get; set; }

    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}