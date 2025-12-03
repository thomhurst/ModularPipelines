using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "event-subscription", "delete", "(eventgrid", "extension)")]
public record AzEventgridEventSubscriptionDeleteEventgridExtensionOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--source-resource-id")]
    public string? SourceResourceId { get; set; }
}