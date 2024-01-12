using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "scopes", "describe")]
public record GcloudContainerHubScopesDescribeOptions(
[property: PositionalArgument] string Scope,
[property: PositionalArgument] string Location
) : GcloudOptions;