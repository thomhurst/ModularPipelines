using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "put-project-events")]
public record AwsEvidentlyPutProjectEventsOptions(
[property: CliOption("--events")] string[] Events,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}