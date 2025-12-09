using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "update-kx-environment-network")]
public record AwsFinspaceUpdateKxEnvironmentNetworkOptions(
[property: CliOption("--environment-id")] string EnvironmentId
) : AwsOptions
{
    [CliOption("--transit-gateway-configuration")]
    public string? TransitGatewayConfiguration { get; set; }

    [CliOption("--custom-dns-configuration")]
    public string[]? CustomDnsConfiguration { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}