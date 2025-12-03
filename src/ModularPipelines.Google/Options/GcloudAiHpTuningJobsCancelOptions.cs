using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "hp-tuning-jobs", "cancel")]
public record GcloudAiHpTuningJobsCancelOptions(
[property: CliArgument] string HptuningJob,
[property: CliArgument] string Region
) : GcloudOptions;