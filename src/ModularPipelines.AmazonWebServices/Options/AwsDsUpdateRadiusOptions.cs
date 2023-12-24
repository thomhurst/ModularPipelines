using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "update-radius")]
public record AwsDsUpdateRadiusOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--radius-settings")] string RadiusSettings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}