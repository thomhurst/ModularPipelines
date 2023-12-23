using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "delete-config")]
public record AwsGroundstationDeleteConfigOptions(
[property: CommandSwitch("--config-id")] string ConfigId,
[property: CommandSwitch("--config-type")] string ConfigType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}