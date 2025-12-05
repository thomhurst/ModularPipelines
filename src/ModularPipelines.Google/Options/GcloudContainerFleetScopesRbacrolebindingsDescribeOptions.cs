using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "scopes", "rbacrolebindings", "describe")]
public record GcloudContainerFleetScopesRbacrolebindingsDescribeOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Location,
[property: CliArgument] string Scope
) : GcloudOptions;