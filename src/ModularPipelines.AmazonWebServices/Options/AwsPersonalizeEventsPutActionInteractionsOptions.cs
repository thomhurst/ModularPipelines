using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize-events", "put-action-interactions")]
public record AwsPersonalizeEventsPutActionInteractionsOptions(
[property: CliOption("--tracking-id")] string TrackingId,
[property: CliOption("--action-interactions")] string[] ActionInteractions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}