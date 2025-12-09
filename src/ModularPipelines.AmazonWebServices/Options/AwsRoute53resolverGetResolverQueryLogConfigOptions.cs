using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "get-resolver-query-log-config")]
public record AwsRoute53resolverGetResolverQueryLogConfigOptions(
[property: CliOption("--resolver-query-log-config-id")] string ResolverQueryLogConfigId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}