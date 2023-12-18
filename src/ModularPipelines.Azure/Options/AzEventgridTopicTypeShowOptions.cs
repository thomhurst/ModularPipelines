using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type", "show")]
public record AzEventgridTopicTypeShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

