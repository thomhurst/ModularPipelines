using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datashare", "email-registration", "activate-email")]
public record AzDatashareEmailRegistrationActivateEmailOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--activation-code")]
    public string? ActivationCode { get; set; }
}