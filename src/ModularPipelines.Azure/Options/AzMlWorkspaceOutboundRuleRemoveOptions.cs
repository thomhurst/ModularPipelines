using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "workspace", "outbound-rule", "remove")]
public record AzMlWorkspaceOutboundRuleRemoveOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule")] string Rule,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}