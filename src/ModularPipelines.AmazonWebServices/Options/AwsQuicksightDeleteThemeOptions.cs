using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-theme")]
public record AwsQuicksightDeleteThemeOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--theme-id")] string ThemeId
) : AwsOptions
{
    [CliOption("--version-number")]
    public long? VersionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}