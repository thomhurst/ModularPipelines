using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "update-firewall-policy")]
public record AwsNetworkFirewallUpdateFirewallPolicyOptions(
[property: CommandSwitch("--update-token")] string UpdateToken
) : AwsOptions
{
    [CommandSwitch("--firewall-policy-arn")]
    public string? FirewallPolicyArn { get; set; }

    [CommandSwitch("--firewall-policy-name")]
    public string? FirewallPolicyName { get; set; }

    [CommandSwitch("--firewall-policy")]
    public string? FirewallPolicy { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--encryption-configuration")]
    public string? EncryptionConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}