using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scheduler", "jobs", "resume")]
public record GcloudSchedulerJobsResumeOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Location
) : GcloudOptions;