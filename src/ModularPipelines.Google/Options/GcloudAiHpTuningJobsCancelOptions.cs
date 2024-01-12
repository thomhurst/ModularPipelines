using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "hp-tuning-jobs", "cancel")]
public record GcloudAiHpTuningJobsCancelOptions(
[property: PositionalArgument] string HptuningJob,
[property: PositionalArgument] string Region
) : GcloudOptions;