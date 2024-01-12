using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "custom-jobs", "describe")]
public record GcloudAiCustomJobsDescribeOptions(
[property: PositionalArgument] string CustomJob,
[property: PositionalArgument] string Region
) : GcloudOptions;