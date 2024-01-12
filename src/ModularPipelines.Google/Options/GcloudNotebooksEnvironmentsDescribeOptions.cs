using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "environments", "describe")]
public record GcloudNotebooksEnvironmentsDescribeOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Location
) : GcloudOptions;