using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type", "show", "(eventgrid", "extension)")]
public record AzEventgridTopicTypeShowEventgridExtensionOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;