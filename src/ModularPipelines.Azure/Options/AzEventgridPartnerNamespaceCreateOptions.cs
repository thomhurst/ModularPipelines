using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "create")]
public record AzEventgridPartnerNamespaceCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partner-registration-id")] string PartnerRegistrationId,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--partner-topic-routing-mode")]
    public string? PartnerTopicRoutingMode { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

