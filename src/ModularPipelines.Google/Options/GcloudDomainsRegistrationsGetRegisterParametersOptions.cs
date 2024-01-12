using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "get-register-parameters")]
public record GcloudDomainsRegistrationsGetRegisterParametersOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions;