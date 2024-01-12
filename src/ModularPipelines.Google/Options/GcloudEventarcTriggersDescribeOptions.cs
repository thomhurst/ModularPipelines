using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "triggers", "describe")]
public record GcloudEventarcTriggersDescribeOptions(
[property: PositionalArgument] string Trigger,
[property: PositionalArgument] string Location
) : GcloudOptions;