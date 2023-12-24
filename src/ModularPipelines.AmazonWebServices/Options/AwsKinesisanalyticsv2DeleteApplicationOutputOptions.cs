using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "delete-application-output")]
public record AwsKinesisanalyticsv2DeleteApplicationOutputOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--current-application-version-id")] long CurrentApplicationVersionId,
[property: CommandSwitch("--output-id")] string OutputId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}