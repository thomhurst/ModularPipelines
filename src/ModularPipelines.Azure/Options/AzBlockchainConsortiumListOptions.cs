using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("blockchain", "consortium", "list")]
public record AzBlockchainConsortiumListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;