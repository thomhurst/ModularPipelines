using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "update-config")]
public record AwsGroundstationUpdateConfigOptions(
[property: CommandSwitch("--config-data")] string ConfigData,
[property: CommandSwitch("--config-id")] string ConfigId,
[property: CommandSwitch("--config-type")] string ConfigType,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}