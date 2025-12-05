using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "providers", "describe")]
public record GcloudEventarcProvidersDescribeOptions(
[property: CliArgument] string Provider,
[property: CliArgument] string Location
) : GcloudOptions;