using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "update-browser-settings")]
public record AwsWorkspacesWebUpdateBrowserSettingsOptions(
[property: CommandSwitch("--browser-settings-arn")] string BrowserSettingsArn
) : AwsOptions
{
    [CommandSwitch("--browser-policy")]
    public string? BrowserPolicy { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}