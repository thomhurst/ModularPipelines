using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "associate-browser-settings")]
public record AwsWorkspacesWebAssociateBrowserSettingsOptions(
[property: CommandSwitch("--browser-settings-arn")] string BrowserSettingsArn,
[property: CommandSwitch("--portal-arn")] string PortalArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}