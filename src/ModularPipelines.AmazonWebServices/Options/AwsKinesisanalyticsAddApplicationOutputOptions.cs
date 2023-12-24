using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalytics", "add-application-output")]
public record AwsKinesisanalyticsAddApplicationOutputOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--current-application-version-id")] long CurrentApplicationVersionId,
[property: CommandSwitch("--application-output")] string ApplicationOutput
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}