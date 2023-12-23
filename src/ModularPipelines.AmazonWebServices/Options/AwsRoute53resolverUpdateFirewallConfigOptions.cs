using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "update-firewall-config")]
public record AwsRoute53resolverUpdateFirewallConfigOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--firewall-fail-open")] string FirewallFailOpen
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}