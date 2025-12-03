using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "configure", "dns")]
public record GcloudDomainsRegistrationsConfigureDnsOptions(
[property: CliArgument] string Registration
) : GcloudOptions;