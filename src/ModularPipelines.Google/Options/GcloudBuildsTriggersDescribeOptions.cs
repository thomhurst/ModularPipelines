using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "triggers", "describe")]
public record GcloudBuildsTriggersDescribeOptions(
[property: CliArgument] string Trigger,
[property: CliArgument] string Region
) : GcloudOptions;