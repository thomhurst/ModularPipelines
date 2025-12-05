using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-blueprint-run")]
public record AwsGlueGetBlueprintRunOptions(
[property: CliOption("--blueprint-name")] string BlueprintName,
[property: CliOption("--run-id")] string RunId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}