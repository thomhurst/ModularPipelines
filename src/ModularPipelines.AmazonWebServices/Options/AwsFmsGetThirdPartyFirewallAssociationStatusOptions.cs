using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fms", "get-third-party-firewall-association-status")]
public record AwsFmsGetThirdPartyFirewallAssociationStatusOptions(
[property: CliOption("--third-party-firewall")] string ThirdPartyFirewall
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}