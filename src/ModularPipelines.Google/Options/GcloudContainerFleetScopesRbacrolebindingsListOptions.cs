using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "scopes", "rbacrolebindings", "list")]
public record GcloudContainerFleetScopesRbacrolebindingsListOptions(
[property: CommandSwitch("--scope")] string Scope
) : GcloudOptions;