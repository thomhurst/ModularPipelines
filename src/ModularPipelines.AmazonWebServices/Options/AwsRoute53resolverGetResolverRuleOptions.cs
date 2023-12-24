using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "get-resolver-rule")]
public record AwsRoute53resolverGetResolverRuleOptions(
[property: CommandSwitch("--resolver-rule-id")] string ResolverRuleId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}