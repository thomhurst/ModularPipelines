using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blockchain", "consortium", "list")]
public record AzBlockchainConsortiumListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}