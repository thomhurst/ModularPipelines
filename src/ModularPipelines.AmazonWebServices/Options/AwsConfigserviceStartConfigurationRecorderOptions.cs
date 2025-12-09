using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "start-configuration-recorder")]
public record AwsConfigserviceStartConfigurationRecorderOptions(
[property: CliOption("--configuration-recorder-name")] string ConfigurationRecorderName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}