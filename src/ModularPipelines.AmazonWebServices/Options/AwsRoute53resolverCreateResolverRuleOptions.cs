using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "create-resolver-rule")]
public record AwsRoute53resolverCreateResolverRuleOptions(
[property: CommandSwitch("--creator-request-id")] string CreatorRequestId,
[property: CommandSwitch("--rule-type")] string RuleType
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--domain-name")]
    public string? DomainName { get; set; }

    [CommandSwitch("--target-ips")]
    public string[]? TargetIps { get; set; }

    [CommandSwitch("--resolver-endpoint-id")]
    public string? ResolverEndpointId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}