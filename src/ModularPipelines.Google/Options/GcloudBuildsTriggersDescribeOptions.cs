using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "describe")]
public record GcloudBuildsTriggersDescribeOptions(
[property: PositionalArgument] string Trigger,
[property: PositionalArgument] string Region
) : GcloudOptions;