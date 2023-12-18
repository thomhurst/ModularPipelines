using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type", "show", "(eventgrid", "extension)")]
public record AzEventgridTopicTypeShowEventgridExtensionOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

