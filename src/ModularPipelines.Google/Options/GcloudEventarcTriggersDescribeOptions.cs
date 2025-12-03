using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "triggers", "describe")]
public record GcloudEventarcTriggersDescribeOptions(
[property: CliArgument] string Trigger,
[property: CliArgument] string Location
) : GcloudOptions;