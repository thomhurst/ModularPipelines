using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "audit-logs-provider", "service-names", "list")]
public record GcloudEventarcAuditLogsProviderServiceNamesListOptions : GcloudOptions;