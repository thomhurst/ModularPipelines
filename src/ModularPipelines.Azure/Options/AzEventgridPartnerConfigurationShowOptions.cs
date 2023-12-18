using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "configuration", "show")]
public record AzEventgridPartnerConfigurationShowOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--auth-exp-date")]
    public string? AuthExpDate { get; set; }

    [CommandSwitch("--partner-name")]
    public string? PartnerName { get; set; }

    [CommandSwitch("--partner-registration-immutable-id")]
    public string? PartnerRegistrationImmutableId { get; set; }
}

