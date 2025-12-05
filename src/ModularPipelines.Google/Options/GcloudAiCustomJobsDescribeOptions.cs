using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "custom-jobs", "describe")]
public record GcloudAiCustomJobsDescribeOptions(
[property: CliArgument] string CustomJob,
[property: CliArgument] string Region
) : GcloudOptions;