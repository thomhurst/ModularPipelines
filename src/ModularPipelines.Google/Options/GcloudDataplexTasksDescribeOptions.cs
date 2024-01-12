using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "tasks", "describe")]
public record GcloudDataplexTasksDescribeOptions(
[property: PositionalArgument] string Task,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions;