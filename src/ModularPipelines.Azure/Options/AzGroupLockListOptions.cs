using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("group", "lock", "list")]
public record AzGroupLockListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--filter-string")]
    public string? FilterString { get; set; }
}