using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "get-resolver-query-log-config-association")]
public record AwsRoute53resolverGetResolverQueryLogConfigAssociationOptions(
[property: CliOption("--resolver-query-log-config-association-id")] string ResolverQueryLogConfigAssociationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}