using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "associate-user-settings")]
public record AwsWorkspacesWebAssociateUserSettingsOptions(
[property: CommandSwitch("--portal-arn")] string PortalArn,
[property: CommandSwitch("--user-settings-arn")] string UserSettingsArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}