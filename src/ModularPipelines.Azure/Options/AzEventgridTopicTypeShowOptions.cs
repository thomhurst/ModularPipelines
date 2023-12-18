using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type", "show")]
public record AzEventgridTopicTypeShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;