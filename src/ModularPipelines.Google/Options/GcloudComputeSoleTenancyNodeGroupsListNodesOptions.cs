using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-groups", "list-nodes")]
public record GcloudComputeSoleTenancyNodeGroupsListNodesOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}