using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("osis", "get-pipeline-blueprint")]
public record AwsOsisGetPipelineBlueprintOptions(
[property: CommandSwitch("--blueprint-name")] string BlueprintName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}