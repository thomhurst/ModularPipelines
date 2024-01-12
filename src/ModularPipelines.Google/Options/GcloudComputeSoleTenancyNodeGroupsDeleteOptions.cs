using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-groups", "delete")]
public record GcloudComputeSoleTenancyNodeGroupsDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}