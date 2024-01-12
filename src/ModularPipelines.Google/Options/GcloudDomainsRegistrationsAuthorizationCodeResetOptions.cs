using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "authorization-code", "reset")]
public record GcloudDomainsRegistrationsAuthorizationCodeResetOptions(
[property: PositionalArgument] string Registration
) : GcloudOptions;