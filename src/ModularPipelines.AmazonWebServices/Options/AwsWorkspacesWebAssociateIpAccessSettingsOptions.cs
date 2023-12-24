using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "associate-ip-access-settings")]
public record AwsWorkspacesWebAssociateIpAccessSettingsOptions(
[property: CommandSwitch("--ip-access-settings-arn")] string IpAccessSettingsArn,
[property: CommandSwitch("--portal-arn")] string PortalArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}