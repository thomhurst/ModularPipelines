using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "authorization-code", "reset")]
public record GcloudDomainsRegistrationsAuthorizationCodeResetOptions(
[property: CliArgument] string Registration
) : GcloudOptions;