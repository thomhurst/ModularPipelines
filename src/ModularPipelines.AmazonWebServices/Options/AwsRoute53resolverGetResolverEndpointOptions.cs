using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "get-resolver-endpoint")]
public record AwsRoute53resolverGetResolverEndpointOptions(
[property: CliOption("--resolver-endpoint-id")] string ResolverEndpointId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}