using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "rbacrolebindings", "list")]
public record GcloudContainerHubScopesRbacrolebindingsListOptions(
[property: CommandSwitch("--scope")] string Scope
) : GcloudOptions;