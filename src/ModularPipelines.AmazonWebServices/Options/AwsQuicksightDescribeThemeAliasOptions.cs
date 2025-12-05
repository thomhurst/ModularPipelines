using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-theme-alias")]
public record AwsQuicksightDescribeThemeAliasOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--theme-id")] string ThemeId,
[property: CliOption("--alias-name")] string AliasName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}