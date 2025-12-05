using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "topic", "event-subscription", "show")]
public record AzEventgridTopicEventSubscriptionShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions
{
    [CliFlag("--full-ed-url")]
    public bool? FullEdUrl { get; set; }

    [CliFlag("--include-attrib-secret")]
    public bool? IncludeAttribSecret { get; set; }
}