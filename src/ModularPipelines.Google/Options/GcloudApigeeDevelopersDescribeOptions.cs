using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "developers", "describe")]
public record GcloudApigeeDevelopersDescribeOptions(
[property: PositionalArgument] string Developer,
[property: PositionalArgument] string Organization
) : GcloudOptions;