using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "partner", "configuration", "authorize")]
public record AzEventgridPartnerConfigurationAuthorizeOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--auth-exp-date")]
    public string? AuthExpDate { get; set; }

    [CliOption("--partner-name")]
    public string? PartnerName { get; set; }

    [CliOption("--partner-registration-immutable-id")]
    public string? PartnerRegistrationImmutableId { get; set; }
}