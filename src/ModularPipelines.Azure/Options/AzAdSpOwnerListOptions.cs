using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "sp", "owner", "list")]
public record AzAdSpOwnerListOptions(
[property: CliOption("--id")] string Id
) : AzOptions;