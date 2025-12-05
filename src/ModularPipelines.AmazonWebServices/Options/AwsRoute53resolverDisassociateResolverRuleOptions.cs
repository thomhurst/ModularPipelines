using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "disassociate-resolver-rule")]
public record AwsRoute53resolverDisassociateResolverRuleOptions(
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--resolver-rule-id")] string ResolverRuleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}