using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("osis", "validate-pipeline")]
public record AwsOsisValidatePipelineOptions(
[property: CommandSwitch("--pipeline-configuration-body")] string PipelineConfigurationBody
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}