using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "disassociate-resolver-rule")]
public record AwsRoute53resolverDisassociateResolverRuleOptions(
[property: CommandSwitch("--vpc-id")] string VpcId,
[property: CommandSwitch("--resolver-rule-id")] string ResolverRuleId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}