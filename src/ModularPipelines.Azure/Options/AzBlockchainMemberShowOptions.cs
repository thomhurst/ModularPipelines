using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blockchain", "member", "show")]
public record AzBlockchainMemberShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;