using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "registration", "create", "(eventgrid", "extension)")]
public record AzEventgridPartnerRegistrationCreateEventgridExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-name")] string PartnerName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-type-name")] string ResourceTypeName
) : AzOptions
{
    [CliOption("--authorized-subscription-ids")]
    public string? AuthorizedSubscriptionIds { get; set; }

    [CliOption("--customer-service-extension")]
    public string? CustomerServiceExtension { get; set; }

    [CliOption("--customer-service-number")]
    public string? CustomerServiceNumber { get; set; }

    [CliOption("--customer-service-uri")]
    public string? CustomerServiceUri { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--logo-uri")]
    public string? LogoUri { get; set; }

    [CliOption("--long-description")]
    public string? LongDescription { get; set; }

    [CliOption("--setup-uri")]
    public string? SetupUri { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}