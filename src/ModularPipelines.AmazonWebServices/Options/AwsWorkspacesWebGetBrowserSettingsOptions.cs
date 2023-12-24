using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "get-browser-settings")]
public record AwsWorkspacesWebGetBrowserSettingsOptions(
[property: CommandSwitch("--browser-settings-arn")] string BrowserSettingsArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}