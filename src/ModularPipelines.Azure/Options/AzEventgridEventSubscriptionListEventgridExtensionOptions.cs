using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "event-subscription", "list", "(eventgrid", "extension)")]
public record AzEventgridEventSubscriptionListEventgridExtensionOptions : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--odata-query")]
    public string? OdataQuery { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--source-resource-id")]
    public string? SourceResourceId { get; set; }

    [CommandSwitch("--topic-type-name")]
    public string? TopicTypeName { get; set; }
}