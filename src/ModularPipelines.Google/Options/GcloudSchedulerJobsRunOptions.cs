using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scheduler", "jobs", "run")]
public record GcloudSchedulerJobsRunOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Location
) : GcloudOptions;