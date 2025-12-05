using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "list-importable-domains")]
public record GcloudDomainsRegistrationsListImportableDomainsOptions : GcloudOptions;