using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "audit-logs-provider", "method-names", "list")]
public record GcloudEventarcAuditLogsProviderMethodNamesListOptions(
[property: CliOption("--service-name")] string ServiceName
) : GcloudOptions;