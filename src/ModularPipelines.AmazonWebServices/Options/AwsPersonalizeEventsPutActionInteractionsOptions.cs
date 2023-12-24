using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize-events", "put-action-interactions")]
public record AwsPersonalizeEventsPutActionInteractionsOptions(
[property: CommandSwitch("--tracking-id")] string TrackingId,
[property: CommandSwitch("--action-interactions")] string[] ActionInteractions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}