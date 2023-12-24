using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "delete-user-access-logging-settings")]
public record AwsWorkspacesWebDeleteUserAccessLoggingSettingsOptions(
[property: CommandSwitch("--user-access-logging-settings-arn")] string UserAccessLoggingSettingsArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}