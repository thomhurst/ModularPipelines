using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "create-dashboard")]
public record AwsQuicksightCreateDashboardOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--dashboard-id")] string DashboardId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--source-entity")]
    public string? SourceEntity { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--version-description")]
    public string? VersionDescription { get; set; }

    [CommandSwitch("--dashboard-publish-options")]
    public string? DashboardPublishOptions { get; set; }

    [CommandSwitch("--theme-arn")]
    public string? ThemeArn { get; set; }

    [CommandSwitch("--definition")]
    public string? Definition { get; set; }

    [CommandSwitch("--validation-strategy")]
    public string? ValidationStrategy { get; set; }

    [CommandSwitch("--folder-arns")]
    public string[]? FolderArns { get; set; }

    [CommandSwitch("--link-sharing-configuration")]
    public string? LinkSharingConfiguration { get; set; }

    [CommandSwitch("--link-entities")]
    public string[]? LinkEntities { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}