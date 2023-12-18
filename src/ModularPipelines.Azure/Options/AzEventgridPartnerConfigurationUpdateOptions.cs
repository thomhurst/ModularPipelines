using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "configuration", "update")]
public record AzEventgridPartnerConfigurationUpdateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--default-maximum-expiration-time-in-days")]
    public string? DefaultMaximumExpirationTimeInDays { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}