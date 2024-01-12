using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "hp-tuning-jobs", "describe")]
public record GcloudAiHpTuningJobsDescribeOptions(
[property: PositionalArgument] string HptuningJob,
[property: PositionalArgument] string Region
) : GcloudOptions;