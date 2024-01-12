using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "authorization-code", "get")]
public record GcloudDomainsRegistrationsAuthorizationCodeGetOptions(
[property: PositionalArgument] string Registration
) : GcloudOptions;