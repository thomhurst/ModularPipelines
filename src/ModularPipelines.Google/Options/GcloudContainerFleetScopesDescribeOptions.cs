using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "scopes", "describe")]
public record GcloudContainerFleetScopesDescribeOptions(
[property: PositionalArgument] string Scope,
[property: PositionalArgument] string Location
) : GcloudOptions;