using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "scopes", "rbacrolebindings", "update")]
public record GcloudContainerFleetScopesRbacrolebindingsUpdateOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Scope
) : GcloudOptions
{
    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }
}