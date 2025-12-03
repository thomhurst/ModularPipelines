using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "triggers", "delete")]
public record GcloudBuildsTriggersDeleteOptions(
[property: CliArgument] string Trigger,
[property: CliArgument] string Region
) : GcloudOptions;