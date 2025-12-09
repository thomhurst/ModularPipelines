using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("osis", "get-pipeline-blueprint")]
public record AwsOsisGetPipelineBlueprintOptions(
[property: CliOption("--blueprint-name")] string BlueprintName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}