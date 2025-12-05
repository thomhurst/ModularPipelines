using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-trigger")]
public record AwsGlueUpdateTriggerOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--trigger-update")] string TriggerUpdate
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}