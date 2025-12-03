using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-dashboard")]
public record AwsQuicksightCreateDashboardOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--dashboard-id")] string DashboardId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--parameters")]
    public string? Parameters { get; set; }

    [CliOption("--permissions")]
    public string[]? Permissions { get; set; }

    [CliOption("--source-entity")]
    public string? SourceEntity { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

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

    [CliOption("--folder-arns")]
    public string[]? FolderArns { get; set; }

    [CliOption("--link-sharing-configuration")]
    public string? LinkSharingConfiguration { get; set; }

    [CliOption("--link-entities")]
    public string[]? LinkEntities { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}