using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "tasks", "jobs", "describe")]
public record GcloudDataplexTasksJobsDescribeOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string Task
) : GcloudOptions;