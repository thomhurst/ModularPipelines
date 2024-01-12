using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "operations", "describe")]
public record GcloudTransferOperationsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;