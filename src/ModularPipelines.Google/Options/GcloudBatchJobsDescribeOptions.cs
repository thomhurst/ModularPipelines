using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "jobs", "describe")]
public record GcloudBatchJobsDescribeOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Location
) : GcloudOptions;