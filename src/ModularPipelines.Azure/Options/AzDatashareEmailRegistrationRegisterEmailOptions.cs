using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "email-registration", "register-email")]
public record AzDatashareEmailRegistrationRegisterEmailOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;