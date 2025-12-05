using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "simulator", "replay-recent-access")]
public record GcloudIamSimulatorReplayRecentAccessOptions(
[property: CliArgument] string Resource,
[property: CliArgument] string PolicyFile
) : GcloudOptions;