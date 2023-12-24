using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "delete-data-quality-job-definition")]
public record AwsSagemakerDeleteDataQualityJobDefinitionOptions(
[property: CommandSwitch("--job-definition-name")] string JobDefinitionName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}