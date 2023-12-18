using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type", "list-event-types", "(eventgrid", "extension)")]
public record AzEventgridTopicTypeListEventTypesEventgridExtensionOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}