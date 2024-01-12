using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "scopes", "rbacrolebindings", "create")]
public record GcloudContainerFleetScopesRbacrolebindingsCreateOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Scope,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--group")] string Group,
[property: CommandSwitch("--user")] string User
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}