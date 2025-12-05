using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "rbacrolebindings", "describe")]
public record GcloudContainerHubScopesRbacrolebindingsDescribeOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Location,
[property: CliArgument] string Scope
) : GcloudOptions;