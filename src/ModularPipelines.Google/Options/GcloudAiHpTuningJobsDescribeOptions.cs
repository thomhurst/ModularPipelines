using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "hp-tuning-jobs", "describe")]
public record GcloudAiHpTuningJobsDescribeOptions(
[property: CliArgument] string HptuningJob,
[property: CliArgument] string Region
) : GcloudOptions;