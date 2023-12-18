using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "system-topic", "event-subscription", "update")]
public record AzEventgridSystemTopicEventSubscriptionUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--system-topic-name")] string SystemTopicName
) : AzOptions
{
    [CommandSwitch("--advanced-filter")]
    public string? AdvancedFilter { get; set; }

    [CommandSwitch("--deadletter-endpoint")]
    public string? DeadletterEndpoint { get; set; }

    [CommandSwitch("--delivery-attribute-mapping")]
    public string? DeliveryAttributeMapping { get; set; }

    [BooleanCommandSwitch("--enable-advanced-filtering-on-arrays")]
    public bool? EnableAdvancedFilteringOnArrays { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CommandSwitch("--included-event-types")]
    public string? IncludedEventTypes { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--qttl")]
    public string? Qttl { get; set; }

    [CommandSwitch("--subject-begins-with")]
    public string? SubjectBeginsWith { get; set; }

    [CommandSwitch("--subject-ends-with")]
    public string? SubjectEndsWith { get; set; }
}

