using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "event-subscription", "update", "(eventgrid", "extension)")]
public record AzEventgridEventSubscriptionUpdateEventgridExtensionOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--advanced-filter")]
    public string? AdvancedFilter { get; set; }

    [CliOption("--deadletter-endpoint")]
    public string? DeadletterEndpoint { get; set; }

    [CliOption("--deadletter-identity")]
    public string? DeadletterIdentity { get; set; }

    [CliOption("--deadletter-identity-endpoint")]
    public string? DeadletterIdentityEndpoint { get; set; }

    [CliOption("--delivery-identity")]
    public string? DeliveryIdentity { get; set; }

    [CliOption("--delivery-identity-endpoint")]
    public string? DeliveryIdentityEndpoint { get; set; }

    [CliOption("--delivery-identity-endpoint-type")]
    public string? DeliveryIdentityEndpointType { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--included-event-types")]
    public string? IncludedEventTypes { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--source-resource-id")]
    public string? SourceResourceId { get; set; }

    [CliOption("--subject-begins-with")]
    public string? SubjectBeginsWith { get; set; }

    [CliOption("--subject-ends-with")]
    public string? SubjectEndsWith { get; set; }
}