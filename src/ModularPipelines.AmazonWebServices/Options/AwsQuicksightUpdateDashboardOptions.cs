using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-dashboard")]
public record AwsQuicksightUpdateDashboardOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--dashboard-id")] string DashboardId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--source-entity")]
    public string? SourceEntity { get; set; }

    [CliOption("--parameters")]
    public string? Parameters { get; set; }

    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliOption("--dashboard-publish-options")]
    public string? DashboardPublishOptions { get; set; }

    [CliOption("--theme-arn")]
    public string? ThemeArn { get; set; }

    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--validation-strategy")]
    public string? ValidationStrategy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}