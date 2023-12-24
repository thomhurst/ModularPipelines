using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalytics", "start-application")]
public record AwsKinesisanalyticsStartApplicationOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--input-configurations")] string[] InputConfigurations
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}