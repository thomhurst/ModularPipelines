using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-blueprint")]
public record AwsGlueUpdateBlueprintOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--blueprint-location")] string BlueprintLocation
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}