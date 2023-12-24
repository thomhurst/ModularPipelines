using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "put-retention-settings")]
public record AwsChimePutRetentionSettingsOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--retention-settings")] string RetentionSettings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}