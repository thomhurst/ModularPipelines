using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "associate-resolver-endpoint-ip-address")]
public record AwsRoute53resolverAssociateResolverEndpointIpAddressOptions(
[property: CommandSwitch("--resolver-endpoint-id")] string ResolverEndpointId,
[property: CommandSwitch("--ip-address")] string IpAddress
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}