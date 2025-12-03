using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "put-resolver-rule-policy")]
public record AwsRoute53resolverPutResolverRulePolicyOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--resolver-rule-policy")] string ResolverRulePolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}