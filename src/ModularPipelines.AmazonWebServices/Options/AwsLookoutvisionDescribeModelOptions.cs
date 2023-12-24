using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutvision", "describe-model")]
public record AwsLookoutvisionDescribeModelOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--model-version")] string ModelVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}