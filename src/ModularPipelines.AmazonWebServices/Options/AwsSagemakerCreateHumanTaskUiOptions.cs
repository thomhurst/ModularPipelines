using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-human-task-ui")]
public record AwsSagemakerCreateHumanTaskUiOptions(
[property: CliOption("--human-task-ui-name")] string HumanTaskUiName,
[property: CliOption("--ui-template")] string UiTemplate
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}