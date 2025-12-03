using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "operations", "list")]
public record GcloudDomainsRegistrationsOperationsListOptions : GcloudOptions;