using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "rbacrolebindings", "list")]
public record GcloudContainerHubScopesRbacrolebindingsListOptions(
[property: CliOption("--scope")] string Scope
) : GcloudOptions;