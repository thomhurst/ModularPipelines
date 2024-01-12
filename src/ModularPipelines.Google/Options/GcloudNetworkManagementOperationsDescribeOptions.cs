using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-management", "operations", "describe")]
public record GcloudNetworkManagementOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;