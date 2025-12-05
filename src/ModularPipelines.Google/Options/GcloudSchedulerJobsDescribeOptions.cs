using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "describe")]
public record GcloudSchedulerJobsDescribeOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location
) : GcloudOptions;