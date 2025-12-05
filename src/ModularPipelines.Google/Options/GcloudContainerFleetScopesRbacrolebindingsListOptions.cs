using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "scopes", "rbacrolebindings", "list")]
public record GcloudContainerFleetScopesRbacrolebindingsListOptions(
[property: CliOption("--scope")] string Scope
) : GcloudOptions;