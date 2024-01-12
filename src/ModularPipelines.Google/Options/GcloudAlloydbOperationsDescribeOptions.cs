using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "operations", "describe")]
public record GcloudAlloydbOperationsDescribeOptions(
[property: PositionalArgument] string Operation,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;