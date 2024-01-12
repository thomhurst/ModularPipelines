using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "operations", "cancel")]
public record GcloudAlloydbOperationsCancelOptions(
[property: PositionalArgument] string Operation,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;