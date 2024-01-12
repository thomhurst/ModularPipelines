using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "audit-logs-provider", "method-names", "list")]
public record GcloudEventarcAuditLogsProviderMethodNamesListOptions(
[property: CommandSwitch("--service-name")] string ServiceName
) : GcloudOptions;