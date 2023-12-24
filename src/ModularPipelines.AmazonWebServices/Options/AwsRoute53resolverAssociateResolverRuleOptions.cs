using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "associate-resolver-rule")]
public record AwsRoute53resolverAssociateResolverRuleOptions(
[property: CommandSwitch("--resolver-rule-id")] string ResolverRuleId,
[property: CommandSwitch("--vpc-id")] string VpcId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}