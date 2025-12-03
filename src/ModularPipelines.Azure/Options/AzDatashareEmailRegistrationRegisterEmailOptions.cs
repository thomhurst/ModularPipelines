using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datashare", "email-registration", "register-email")]
public record AzDatashareEmailRegistrationRegisterEmailOptions(
[property: CliOption("--location")] string Location
) : AzOptions;