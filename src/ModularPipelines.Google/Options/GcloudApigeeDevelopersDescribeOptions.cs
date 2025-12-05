using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "developers", "describe")]
public record GcloudApigeeDevelopersDescribeOptions(
[property: CliArgument] string Developer,
[property: CliArgument] string Organization
) : GcloudOptions;