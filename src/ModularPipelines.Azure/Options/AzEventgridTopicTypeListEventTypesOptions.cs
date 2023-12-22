using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type", "list-event-types")]
public record AzEventgridTopicTypeListEventTypesOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;