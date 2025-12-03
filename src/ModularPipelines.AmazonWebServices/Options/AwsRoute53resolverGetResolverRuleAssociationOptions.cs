using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "get-resolver-rule-association")]
public record AwsRoute53resolverGetResolverRuleAssociationOptions(
[property: CliOption("--resolver-rule-association-id")] string ResolverRuleAssociationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}