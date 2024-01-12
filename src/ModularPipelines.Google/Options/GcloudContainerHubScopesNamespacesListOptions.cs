using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "namespaces", "list")]
public record GcloudContainerHubScopesNamespacesListOptions(
[property: CommandSwitch("--scope")] string Scope
) : GcloudOptions;