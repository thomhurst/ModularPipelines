using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "put-resolver-rule-policy")]
public record AwsRoute53resolverPutResolverRulePolicyOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--resolver-rule-policy")] string ResolverRulePolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}