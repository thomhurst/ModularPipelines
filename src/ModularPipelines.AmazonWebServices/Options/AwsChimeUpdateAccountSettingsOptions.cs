using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "update-account-settings")]
public record AwsChimeUpdateAccountSettingsOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--account-settings")] string AccountSettings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}