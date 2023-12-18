using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "email-registration", "register-email")]
public record AzDatashareEmailRegistrationRegisterEmailOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}