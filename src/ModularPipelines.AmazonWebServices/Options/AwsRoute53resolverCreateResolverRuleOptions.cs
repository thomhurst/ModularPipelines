using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "create-resolver-rule")]
public record AwsRoute53resolverCreateResolverRuleOptions(
[property: CliOption("--creator-request-id")] string CreatorRequestId,
[property: CliOption("--rule-type")] string RuleType
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliOption("--target-ips")]
    public string[]? TargetIps { get; set; }

    [CliOption("--resolver-endpoint-id")]
    public string? ResolverEndpointId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}