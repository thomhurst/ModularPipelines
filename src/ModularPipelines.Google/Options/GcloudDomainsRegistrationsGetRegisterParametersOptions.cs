using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "get-register-parameters")]
public record GcloudDomainsRegistrationsGetRegisterParametersOptions(
[property: CliArgument] string Domain
) : GcloudOptions;