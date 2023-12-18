using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "configuration", "unauthorize")]
public record AzEventgridPartnerConfigurationUnauthorizeOptions(
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