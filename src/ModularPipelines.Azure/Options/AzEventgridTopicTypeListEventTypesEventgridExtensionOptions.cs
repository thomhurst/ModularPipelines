using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type", "list-event-types", "(eventgrid", "extension)")]
public record AzEventgridTopicTypeListEventTypesEventgridExtensionOptions(
[property: CliOption("--name")] string Name
) : AzOptions;