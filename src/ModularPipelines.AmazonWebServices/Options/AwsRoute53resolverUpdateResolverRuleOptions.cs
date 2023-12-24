using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "update-resolver-rule")]
public record AwsRoute53resolverUpdateResolverRuleOptions(
[property: CommandSwitch("--resolver-rule-id")] string ResolverRuleId,
[property: CommandSwitch("--config")] string Config
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}