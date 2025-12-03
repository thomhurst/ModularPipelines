using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "remove-targets")]
public record AwsEventsRemoveTargetsOptions(
[property: CliOption("--rule")] string Rule,
[property: CliOption("--ids")] string[] Ids
) : AwsOptions
{
    [CliOption("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}