using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "disassociate-resolver-endpoint-ip-address")]
public record AwsRoute53resolverDisassociateResolverEndpointIpAddressOptions(
[property: CliOption("--resolver-endpoint-id")] string ResolverEndpointId,
[property: CliOption("--ip-address")] string IpAddress
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}