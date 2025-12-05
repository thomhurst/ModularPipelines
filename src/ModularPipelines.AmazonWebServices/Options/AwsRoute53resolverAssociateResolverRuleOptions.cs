using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "associate-resolver-rule")]
public record AwsRoute53resolverAssociateResolverRuleOptions(
[property: CliOption("--resolver-rule-id")] string ResolverRuleId,
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}