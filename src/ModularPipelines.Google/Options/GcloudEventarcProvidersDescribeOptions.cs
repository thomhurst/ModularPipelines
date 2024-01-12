using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "providers", "describe")]
public record GcloudEventarcProvidersDescribeOptions(
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string Location
) : GcloudOptions;