using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "associate-user-access-logging-settings")]
public record AwsWorkspacesWebAssociateUserAccessLoggingSettingsOptions(
[property: CommandSwitch("--portal-arn")] string PortalArn,
[property: CommandSwitch("--user-access-logging-settings-arn")] string UserAccessLoggingSettingsArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}