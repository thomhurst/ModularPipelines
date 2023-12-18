using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "event-subscription", "delete", "(eventgrid", "extension)")]
public record AzEventgridEventSubscriptionDeleteEventgridExtensionOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--source-resource-id")]
    public string? SourceResourceId { get; set; }
}