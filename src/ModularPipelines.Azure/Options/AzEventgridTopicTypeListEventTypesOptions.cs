using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic-type", "list-event-types")]
public record AzEventgridTopicTypeListEventTypesOptions(
[property: CliOption("--name")] string Name
) : AzOptions;