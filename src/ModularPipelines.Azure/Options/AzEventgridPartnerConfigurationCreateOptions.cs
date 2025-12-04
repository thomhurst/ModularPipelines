using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "configuration", "create")]
public record AzEventgridPartnerConfigurationCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--authorized-partner")]
    public string? AuthorizedPartner { get; set; }

    [CliOption("--default-maximum-expiration-time-in-days")]
    public string? DefaultMaximumExpirationTimeInDays { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}