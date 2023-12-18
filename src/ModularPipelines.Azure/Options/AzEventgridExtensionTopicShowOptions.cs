using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "extension-topic", "show")]
public record AzEventgridExtensionTopicShowOptions(
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
}