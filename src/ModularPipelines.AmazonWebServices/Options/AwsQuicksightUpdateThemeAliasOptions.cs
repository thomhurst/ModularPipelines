using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-theme-alias")]
public record AwsQuicksightUpdateThemeAliasOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--theme-id")] string ThemeId,
[property: CommandSwitch("--alias-name")] string AliasName,
[property: CommandSwitch("--theme-version-number")] long ThemeVersionNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}