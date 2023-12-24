using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-service-setting")]
public record AwsSsmUpdateServiceSettingOptions(
[property: CommandSwitch("--setting-id")] string SettingId,
[property: CommandSwitch("--setting-value")] string SettingValue
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}