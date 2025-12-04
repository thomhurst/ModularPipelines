using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "configuration", "update")]
public record AzEventgridPartnerConfigurationUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--default-maximum-expiration-time-in-days")]
    public string? DefaultMaximumExpirationTimeInDays { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}