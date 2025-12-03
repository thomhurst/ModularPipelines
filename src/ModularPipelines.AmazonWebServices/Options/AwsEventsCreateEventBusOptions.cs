using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "create-event-bus")]
public record AwsEventsCreateEventBusOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--event-source-name")]
    public string? EventSourceName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}