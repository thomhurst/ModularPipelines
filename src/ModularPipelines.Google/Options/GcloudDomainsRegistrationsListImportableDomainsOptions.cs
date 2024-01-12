using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("domains", "registrations", "list-importable-domains")]
public record GcloudDomainsRegistrationsListImportableDomainsOptions : GcloudOptions;