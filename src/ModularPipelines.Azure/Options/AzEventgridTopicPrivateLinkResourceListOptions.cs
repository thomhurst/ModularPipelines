using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "topic", "private-link-resource", "list")]
public record AzEventgridTopicPrivateLinkResourceListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions;