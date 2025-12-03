using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "update-firewall-config")]
public record AwsRoute53resolverUpdateFirewallConfigOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--firewall-fail-open")] string FirewallFailOpen
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}