using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "update-resolver-endpoint")]
public record AwsRoute53resolverUpdateResolverEndpointOptions(
[property: CliOption("--resolver-endpoint-id")] string ResolverEndpointId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resolver-endpoint-type")]
    public string? ResolverEndpointType { get; set; }

    [CliOption("--update-ip-addresses")]
    public string[]? UpdateIpAddresses { get; set; }

    [CliOption("--protocols")]
    public string[]? Protocols { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}