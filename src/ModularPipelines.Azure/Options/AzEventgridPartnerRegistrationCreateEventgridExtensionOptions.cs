using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "registration", "create", "(eventgrid", "extension)")]
public record AzEventgridPartnerRegistrationCreateEventgridExtensionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partner-name")] string PartnerName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-type-name")] string ResourceTypeName
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

    [CommandSwitch("--setup-uri")]
    public string? SetupUri { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}