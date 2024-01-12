using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "delete")]
public record GcloudBuildsTriggersDeleteOptions(
[property: PositionalArgument] string Trigger,
[property: PositionalArgument] string Region
) : GcloudOptions;