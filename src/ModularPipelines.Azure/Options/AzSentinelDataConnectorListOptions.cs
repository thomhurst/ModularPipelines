using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "data-connector", "list")]
public record AzSentinelDataConnectorListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;