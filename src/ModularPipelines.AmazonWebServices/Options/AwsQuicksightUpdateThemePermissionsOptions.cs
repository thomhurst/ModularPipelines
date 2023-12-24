using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-theme-permissions")]
public record AwsQuicksightUpdateThemePermissionsOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--theme-id")] string ThemeId
) : AwsOptions
{
    [CommandSwitch("--grant-permissions")]
    public string[]? GrantPermissions { get; set; }

    [CommandSwitch("--revoke-permissions")]
    public string[]? RevokePermissions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}