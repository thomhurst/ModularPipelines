using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "operations", "describe")]
public record GcloudAppOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;