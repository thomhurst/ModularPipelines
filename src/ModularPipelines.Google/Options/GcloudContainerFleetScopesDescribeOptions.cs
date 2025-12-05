using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "scopes", "describe")]
public record GcloudContainerFleetScopesDescribeOptions(
[property: CliArgument] string Scope,
[property: CliArgument] string Location
) : GcloudOptions;