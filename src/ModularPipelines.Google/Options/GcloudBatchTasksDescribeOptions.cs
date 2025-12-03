using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "tasks", "describe")]
public record GcloudBatchTasksDescribeOptions(
[property: CliArgument] string Task,
[property: CliArgument] string Job,
[property: CliArgument] string Location,
[property: CliArgument] string TaskGroup
) : GcloudOptions;