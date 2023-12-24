using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "stop-configuration-recorder")]
public record AwsConfigserviceStopConfigurationRecorderOptions(
[property: CommandSwitch("--configuration-recorder-name")] string ConfigurationRecorderName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}