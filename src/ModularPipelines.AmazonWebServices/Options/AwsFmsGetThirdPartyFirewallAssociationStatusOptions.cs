using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fms", "get-third-party-firewall-association-status")]
public record AwsFmsGetThirdPartyFirewallAssociationStatusOptions(
[property: CommandSwitch("--third-party-firewall")] string ThirdPartyFirewall
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}