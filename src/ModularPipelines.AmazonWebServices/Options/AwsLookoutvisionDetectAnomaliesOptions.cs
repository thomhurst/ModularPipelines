using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutvision", "detect-anomalies")]
public record AwsLookoutvisionDetectAnomaliesOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--model-version")] string ModelVersion,
[property: CommandSwitch("--body")] string Body,
[property: CommandSwitch("--content-type")] string ContentType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}