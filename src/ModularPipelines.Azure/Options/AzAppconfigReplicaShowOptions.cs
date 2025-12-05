using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "replica", "show")]
public record AzAppconfigReplicaShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--store-name")] string StoreName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}