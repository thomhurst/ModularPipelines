using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "rbacrolebindings", "delete")]
public record GcloudContainerHubScopesRbacrolebindingsDeleteOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Location,
[property: CliArgument] string Scope
) : GcloudOptions;