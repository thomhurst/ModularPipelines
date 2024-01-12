using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "configure", "dns")]
public record GcloudDomainsRegistrationsConfigureDnsOptions(
[property: PositionalArgument] string Registration
) : GcloudOptions;