using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "run")]
public record GcloudSchedulerJobsRunOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location
) : GcloudOptions;