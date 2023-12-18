using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "configuration", "create")]
public record AzEventgridPartnerConfigurationCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--authorized-partner")]
    public string? AuthorizedPartner { get; set; }

    [CommandSwitch("--default-maximum-expiration-time-in-days")]
    public string? DefaultMaximumExpirationTimeInDays { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}