using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "update-resolver-endpoint")]
public record AwsRoute53resolverUpdateResolverEndpointOptions(
[property: CommandSwitch("--resolver-endpoint-id")] string ResolverEndpointId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resolver-endpoint-type")]
    public string? ResolverEndpointType { get; set; }

    [CommandSwitch("--update-ip-addresses")]
    public string[]? UpdateIpAddresses { get; set; }

    [CommandSwitch("--protocols")]
    public string[]? Protocols { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}