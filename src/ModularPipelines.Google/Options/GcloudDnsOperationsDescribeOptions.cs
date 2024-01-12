using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "operations", "describe")]
public record GcloudDnsOperationsDescribeOptions(
[property: PositionalArgument] string OperationId,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions;