using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "update-runtime-configuration")]
public record AwsGameliftUpdateRuntimeConfigurationOptions(
[property: CommandSwitch("--fleet-id")] string FleetId,
[property: CommandSwitch("--runtime-configuration")] string RuntimeConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}