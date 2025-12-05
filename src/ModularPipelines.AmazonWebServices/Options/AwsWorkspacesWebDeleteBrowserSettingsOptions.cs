using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "delete-browser-settings")]
public record AwsWorkspacesWebDeleteBrowserSettingsOptions(
[property: CliOption("--browser-settings-arn")] string BrowserSettingsArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}