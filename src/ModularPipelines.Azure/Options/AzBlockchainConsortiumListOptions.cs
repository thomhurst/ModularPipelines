using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blockchain", "consortium", "list")]
public record AzBlockchainConsortiumListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;