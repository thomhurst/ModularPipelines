using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "system-topic", "event-subscription", "list", "(eventgrid", "extension)")]
public record AzEventgridSystemTopicEventSubscriptionListEventgridExtensionOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--system-topic-name")] string SystemTopicName
) : AzOptions
{
    [CommandSwitch("--odata-query")]
    public string? OdataQuery { get; set; }
}