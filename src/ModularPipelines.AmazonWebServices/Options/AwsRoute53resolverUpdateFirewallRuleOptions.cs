using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "update-firewall-rule")]
public record AwsRoute53resolverUpdateFirewallRuleOptions(
[property: CliOption("--firewall-rule-group-id")] string FirewallRuleGroupId,
[property: CliOption("--firewall-domain-list-id")] string FirewallDomainListId
) : AwsOptions
{
    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--block-response")]
    public string? BlockResponse { get; set; }

    [CliOption("--block-override-domain")]
    public string? BlockOverrideDomain { get; set; }

    [CliOption("--block-override-dns-type")]
    public string? BlockOverrideDnsType { get; set; }

    [CliOption("--block-override-ttl")]
    public int? BlockOverrideTtl { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}