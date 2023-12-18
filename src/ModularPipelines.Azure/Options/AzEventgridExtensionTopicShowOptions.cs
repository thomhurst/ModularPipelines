using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "extension-topic", "show")]
public record AzEventgridExtensionTopicShowOptions(
[property: CommandSwitch("--scope")] string Scope
) : AzOptions;