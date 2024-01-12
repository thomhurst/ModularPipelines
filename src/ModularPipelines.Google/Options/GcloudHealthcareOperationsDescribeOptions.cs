using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "operations", "describe")]
public record GcloudHealthcareOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions;