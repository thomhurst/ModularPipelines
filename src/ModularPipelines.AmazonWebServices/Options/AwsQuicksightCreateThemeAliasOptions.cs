using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-theme-alias")]
public record AwsQuicksightCreateThemeAliasOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--theme-id")] string ThemeId,
[property: CliOption("--alias-name")] string AliasName,
[property: CliOption("--theme-version-number")] long ThemeVersionNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}