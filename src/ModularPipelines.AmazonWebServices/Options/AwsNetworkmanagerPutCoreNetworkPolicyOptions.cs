using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "put-core-network-policy")]
public record AwsNetworkmanagerPutCoreNetworkPolicyOptions(
[property: CliOption("--core-network-id")] string CoreNetworkId,
[property: CliOption("--policy-document")] string PolicyDocument
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--latest-version-id")]
    public int? LatestVersionId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}