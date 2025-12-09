using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "namespace", "create")]
public record AzEventgridPartnerNamespaceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-registration-id")] string PartnerRegistrationId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--partner-topic-routing-mode")]
    public string? PartnerTopicRoutingMode { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}