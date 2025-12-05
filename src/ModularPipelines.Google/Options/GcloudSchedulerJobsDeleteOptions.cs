using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "delete")]
public record GcloudSchedulerJobsDeleteOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location
) : GcloudOptions;