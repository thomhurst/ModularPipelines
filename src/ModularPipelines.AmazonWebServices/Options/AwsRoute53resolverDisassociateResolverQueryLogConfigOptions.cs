using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "disassociate-resolver-query-log-config")]
public record AwsRoute53resolverDisassociateResolverQueryLogConfigOptions(
[property: CliOption("--resolver-query-log-config-id")] string ResolverQueryLogConfigId,
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}