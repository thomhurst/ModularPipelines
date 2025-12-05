using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "update-browser-settings")]
public record AwsWorkspacesWebUpdateBrowserSettingsOptions(
[property: CliOption("--browser-settings-arn")] string BrowserSettingsArn
) : AwsOptions
{
    [CliOption("--browser-policy")]
    public string? BrowserPolicy { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}