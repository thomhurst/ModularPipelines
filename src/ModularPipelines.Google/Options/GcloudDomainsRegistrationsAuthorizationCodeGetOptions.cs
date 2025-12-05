using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "authorization-code", "get")]
public record GcloudDomainsRegistrationsAuthorizationCodeGetOptions(
[property: CliArgument] string Registration
) : GcloudOptions;