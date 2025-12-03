using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "resume")]
public record GcloudSchedulerJobsResumeOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location
) : GcloudOptions;