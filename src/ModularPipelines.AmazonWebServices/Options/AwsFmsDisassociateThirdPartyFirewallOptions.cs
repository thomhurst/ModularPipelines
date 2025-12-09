using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fms", "disassociate-third-party-firewall")]
public record AwsFmsDisassociateThirdPartyFirewallOptions(
[property: CliOption("--third-party-firewall")] string ThirdPartyFirewall
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}