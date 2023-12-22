using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("term", "show")]
public record AzTermShowOptions(
[property: CommandSwitch("--plan")] string Plan,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--publisher")] string Publisher
) : AzOptions;