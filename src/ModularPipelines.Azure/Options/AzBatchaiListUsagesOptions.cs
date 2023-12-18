using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai", "list-usages")]
public record AzBatchaiListUsagesOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}