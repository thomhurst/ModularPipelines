using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "associate-browser-settings")]
public record AwsWorkspacesWebAssociateBrowserSettingsOptions(
[property: CliOption("--browser-settings-arn")] string BrowserSettingsArn,
[property: CliOption("--portal-arn")] string PortalArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}