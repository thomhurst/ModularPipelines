using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "simulator", "replay-recent-access")]
public record GcloudIamSimulatorReplayRecentAccessOptions(
[property: PositionalArgument] string Resource,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;