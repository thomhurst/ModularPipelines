using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-theme-alias")]
public record AwsQuicksightUpdateThemeAliasOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--theme-id")] string ThemeId,
[property: CliOption("--alias-name")] string AliasName,
[property: CliOption("--theme-version-number")] long ThemeVersionNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}