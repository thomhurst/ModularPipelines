using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "jobs", "describe")]
public record GcloudBatchJobsDescribeOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location
) : GcloudOptions;