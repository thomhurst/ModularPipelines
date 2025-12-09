using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "restore-core-network-policy-version")]
public record AwsNetworkmanagerRestoreCoreNetworkPolicyVersionOptions(
[property: CliOption("--core-network-id")] string CoreNetworkId,
[property: CliOption("--policy-version-id")] int PolicyVersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}