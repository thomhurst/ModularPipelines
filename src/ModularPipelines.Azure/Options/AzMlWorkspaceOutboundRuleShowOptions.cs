using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "workspace", "outbound-rule", "show")]
public record AzMlWorkspaceOutboundRuleShowOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule")] string Rule,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;