using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "configure", "contacts")]
public record GcloudDomainsRegistrationsConfigureContactsOptions(
[property: CliArgument] string Registration
) : GcloudOptions;