using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "incident", "list-alert")]
public record AzSentinelIncidentListAlertOptions(
[property: CliOption("--incident-id")] string IncidentId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;