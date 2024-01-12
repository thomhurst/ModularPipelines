using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "operations", "describe")]
public record GcloudStorageOperationsDescribeOptions(
[property: PositionalArgument] string OperationName
) : GcloudOptions;