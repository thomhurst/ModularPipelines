using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "memberships", "bindings", "create")]
public record GcloudContainerFleetMembershipsBindingsCreateOptions(
[property: PositionalArgument] string Binding,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Membership,
[property: CommandSwitch("--scope")] string Scope
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}