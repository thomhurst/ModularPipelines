using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "disassociate-resolver-endpoint-ip-address")]
public record AwsRoute53resolverDisassociateResolverEndpointIpAddressOptions(
[property: CommandSwitch("--resolver-endpoint-id")] string ResolverEndpointId,
[property: CommandSwitch("--ip-address")] string IpAddress
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}