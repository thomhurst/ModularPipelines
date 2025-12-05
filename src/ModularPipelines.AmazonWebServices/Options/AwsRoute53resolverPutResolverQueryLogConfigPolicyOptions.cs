using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "put-resolver-query-log-config-policy")]
public record AwsRoute53resolverPutResolverQueryLogConfigPolicyOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--resolver-query-log-config-policy")] string ResolverQueryLogConfigPolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}