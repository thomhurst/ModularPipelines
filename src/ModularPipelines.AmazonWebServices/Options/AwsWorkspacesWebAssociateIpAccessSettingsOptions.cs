using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "associate-ip-access-settings")]
public record AwsWorkspacesWebAssociateIpAccessSettingsOptions(
[property: CliOption("--ip-access-settings-arn")] string IpAccessSettingsArn,
[property: CliOption("--portal-arn")] string PortalArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}