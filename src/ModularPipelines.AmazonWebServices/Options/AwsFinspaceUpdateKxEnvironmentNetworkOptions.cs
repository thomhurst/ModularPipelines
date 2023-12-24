using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "update-kx-environment-network")]
public record AwsFinspaceUpdateKxEnvironmentNetworkOptions(
[property: CommandSwitch("--environment-id")] string EnvironmentId
) : AwsOptions
{
    [CommandSwitch("--transit-gateway-configuration")]
    public string? TransitGatewayConfiguration { get; set; }

    [CommandSwitch("--custom-dns-configuration")]
    public string[]? CustomDnsConfiguration { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}