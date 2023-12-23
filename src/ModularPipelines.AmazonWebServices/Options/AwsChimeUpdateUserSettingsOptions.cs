using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "update-user-settings")]
public record AwsChimeUpdateUserSettingsOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--user-settings")] string UserSettings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}