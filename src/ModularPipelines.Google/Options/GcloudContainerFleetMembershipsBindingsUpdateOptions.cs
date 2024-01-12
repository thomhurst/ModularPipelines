using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "memberships", "bindings", "update")]
public record GcloudContainerFleetMembershipsBindingsUpdateOptions(
[property: PositionalArgument] string Binding,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Membership,
[property: CommandSwitch("--scope")] string Scope
) : GcloudOptions
{
    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}