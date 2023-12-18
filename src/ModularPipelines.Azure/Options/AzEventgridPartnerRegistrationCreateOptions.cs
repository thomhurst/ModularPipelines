using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "registration", "create")]
public record AzEventgridPartnerRegistrationCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--authorized-subscription-ids")]
    public string? AuthorizedSubscriptionIds { get; set; }

    [CommandSwitch("--customer-service-extension")]
    public string? CustomerServiceExtension { get; set; }

    [CommandSwitch("--customer-service-number")]
    public string? CustomerServiceNumber { get; set; }

    [CommandSwitch("--customer-service-uri")]
    public string? CustomerServiceUri { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--logo-uri")]
    public string? LogoUri { get; set; }

    [CommandSwitch("--long-description")]
    public string? LongDescription { get; set; }

    [CommandSwitch("--partner-name")]
    public string? PartnerName { get; set; }

    [CommandSwitch("--resource-type-name")]
    public string? ResourceTypeName { get; set; }

    [CommandSwitch("--setup-uri")]
    public string? SetupUri { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}