using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "delete-configuration-recorder")]
public record AwsConfigserviceDeleteConfigurationRecorderOptions(
[property: CommandSwitch("--configuration-recorder-name")] string ConfigurationRecorderName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}