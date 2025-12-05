using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot1click-projects", "create-placement")]
public record AwsIot1clickProjectsCreatePlacementOptions(
[property: CliOption("--placement-name")] string PlacementName,
[property: CliOption("--project-name")] string ProjectName
) : AwsOptions
{
    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}