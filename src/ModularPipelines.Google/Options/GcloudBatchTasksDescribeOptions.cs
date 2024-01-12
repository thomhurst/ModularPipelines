using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "tasks", "describe")]
public record GcloudBatchTasksDescribeOptions(
[property: PositionalArgument] string Task,
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string TaskGroup
) : GcloudOptions;