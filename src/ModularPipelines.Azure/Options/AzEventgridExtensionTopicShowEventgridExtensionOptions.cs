using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "extension-topic", "show", "(eventgrid", "extension)")]
public record AzEventgridExtensionTopicShowEventgridExtensionOptions(
[property: CommandSwitch("--scope")] string Scope
) : AzOptions;