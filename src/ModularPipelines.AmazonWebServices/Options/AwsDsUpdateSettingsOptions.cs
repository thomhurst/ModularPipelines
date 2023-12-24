using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "update-settings")]
public record AwsDsUpdateSettingsOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--settings")] string[] Settings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}