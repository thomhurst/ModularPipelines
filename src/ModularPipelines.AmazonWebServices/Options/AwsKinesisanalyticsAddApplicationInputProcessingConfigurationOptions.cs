using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalytics", "add-application-input-processing-configuration")]
public record AwsKinesisanalyticsAddApplicationInputProcessingConfigurationOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--current-application-version-id")] long CurrentApplicationVersionId,
[property: CommandSwitch("--input-id")] string InputId,
[property: CommandSwitch("--input-processing-configuration")] string InputProcessingConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}