using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("term", "accept")]
public record AzTermAcceptOptions(
[property: CommandSwitch("--plan")] string Plan,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--publisher")] string Publisher
) : AzOptions
{
}

