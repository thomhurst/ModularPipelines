using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "put-project-events")]
public record AwsEvidentlyPutProjectEventsOptions(
[property: CommandSwitch("--events")] string[] Events,
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}