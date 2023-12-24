using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "create-theme")]
public record AwsQuicksightCreateThemeOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--theme-id")] string ThemeId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--base-theme-id")] string BaseThemeId,
[property: CommandSwitch("--configuration")] string Configuration
) : AwsOptions
{
    [CommandSwitch("--version-description")]
    public string? VersionDescription { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}