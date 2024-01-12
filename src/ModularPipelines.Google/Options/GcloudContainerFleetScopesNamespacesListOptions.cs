using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "scopes", "namespaces", "list")]
public record GcloudContainerFleetScopesNamespacesListOptions(
[property: CommandSwitch("--scope")] string Scope
) : GcloudOptions;