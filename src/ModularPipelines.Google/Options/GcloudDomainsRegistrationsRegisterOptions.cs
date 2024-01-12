using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "register")]
public record GcloudDomainsRegistrationsRegisterOptions(
[property: PositionalArgument] string Registration
) : GcloudOptions;