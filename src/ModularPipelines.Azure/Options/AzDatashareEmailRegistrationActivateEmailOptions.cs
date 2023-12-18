using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "email-registration", "activate-email")]
public record AzDatashareEmailRegistrationActivateEmailOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--activation-code")]
    public string? ActivationCode { get; set; }
}