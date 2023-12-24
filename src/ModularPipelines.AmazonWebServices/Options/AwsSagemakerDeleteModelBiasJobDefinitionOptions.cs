using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "delete-model-bias-job-definition")]
public record AwsSagemakerDeleteModelBiasJobDefinitionOptions(
[property: CommandSwitch("--job-definition-name")] string JobDefinitionName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}