using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "pause")]
public record GcloudSchedulerJobsPauseOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Location
) : GcloudOptions;