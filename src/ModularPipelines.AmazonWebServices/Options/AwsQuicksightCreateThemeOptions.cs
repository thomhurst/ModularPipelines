using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-theme")]
public record AwsQuicksightCreateThemeOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--theme-id")] string ThemeId,
[property: CliOption("--name")] string Name,
[property: CliOption("--base-theme-id")] string BaseThemeId,
[property: CliOption("--configuration")] string Configuration
) : AwsOptions
{
    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliOption("--permissions")]
    public string[]? Permissions { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}