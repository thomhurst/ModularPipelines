using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "search-domains")]
public record GcloudDomainsRegistrationsSearchDomainsOptions(
[property: CliArgument] string DomainQuery
) : GcloudOptions;