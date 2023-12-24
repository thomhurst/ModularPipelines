using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "get-resolver-rule-association")]
public record AwsRoute53resolverGetResolverRuleAssociationOptions(
[property: CommandSwitch("--resolver-rule-association-id")] string ResolverRuleAssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}