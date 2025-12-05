using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "describe")]
public record GcloudContainerHubScopesDescribeOptions(
[property: CliArgument] string Scope,
[property: CliArgument] string Location
) : GcloudOptions;