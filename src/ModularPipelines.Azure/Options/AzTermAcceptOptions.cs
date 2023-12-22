using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("term", "accept")]
public record AzTermAcceptOptions(
[property: CommandSwitch("--plan")] string Plan,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--publisher")] string Publisher
) : AzOptions;