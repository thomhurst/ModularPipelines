using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "create-firewall-rule")]
public record AwsRoute53resolverCreateFirewallRuleOptions(
[property: CliOption("--firewall-rule-group-id")] string FirewallRuleGroupId,
[property: CliOption("--firewall-domain-list-id")] string FirewallDomainListId,
[property: CliOption("--priority")] int Priority,
[property: CliOption("--action")] string Action,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--block-response")]
    public string? BlockResponse { get; set; }

    [CliOption("--block-override-domain")]
    public string? BlockOverrideDomain { get; set; }

    [CliOption("--block-override-dns-type")]
    public string? BlockOverrideDnsType { get; set; }

    [CliOption("--block-override-ttl")]
    public int? BlockOverrideTtl { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}