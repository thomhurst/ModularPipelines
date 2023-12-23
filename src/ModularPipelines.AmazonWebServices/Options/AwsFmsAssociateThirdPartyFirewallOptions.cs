using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fms", "associate-third-party-firewall")]
public record AwsFmsAssociateThirdPartyFirewallOptions(
[property: CommandSwitch("--third-party-firewall")] string ThirdPartyFirewall
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}