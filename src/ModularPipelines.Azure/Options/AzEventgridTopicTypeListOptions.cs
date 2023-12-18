using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type", "list")]
public record AzEventgridTopicTypeListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

