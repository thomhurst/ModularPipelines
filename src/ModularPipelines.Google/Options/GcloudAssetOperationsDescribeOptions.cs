using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "operations", "describe")]
public record GcloudAssetOperationsDescribeOptions(
[property: PositionalArgument] string OperationName
) : GcloudOptions;