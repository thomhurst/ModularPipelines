using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "registration", "create")]
public record AzEventgridPartnerRegistrationCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--authorized-subscription-ids")]
    public string? AuthorizedSubscriptionIds { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}