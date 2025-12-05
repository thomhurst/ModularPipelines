using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivschat", "send-event")]
public record AwsIvschatSendEventOptions(
[property: CliOption("--event-name")] string EventName,
[property: CliOption("--room-identifier")] string RoomIdentifier
) : AwsOptions
{
    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}