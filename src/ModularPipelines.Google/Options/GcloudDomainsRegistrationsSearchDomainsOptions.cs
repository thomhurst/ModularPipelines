using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "search-domains")]
public record GcloudDomainsRegistrationsSearchDomainsOptions(
[property: PositionalArgument] string DomainQuery
) : GcloudOptions;