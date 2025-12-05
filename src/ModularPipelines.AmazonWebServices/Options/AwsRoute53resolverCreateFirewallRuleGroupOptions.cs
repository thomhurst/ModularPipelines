using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "create-firewall-rule-group")]
public record AwsRoute53resolverCreateFirewallRuleGroupOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}