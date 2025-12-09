using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "update-resolver-rule")]
public record AwsRoute53resolverUpdateResolverRuleOptions(
[property: CliOption("--resolver-rule-id")] string ResolverRuleId,
[property: CliOption("--config")] string Config
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}