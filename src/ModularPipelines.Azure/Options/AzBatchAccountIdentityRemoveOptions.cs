using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "account", "identity", "remove")]
public record AzBatchAccountIdentityRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--system-assigned")]
    public string? SystemAssigned { get; set; }

    [CliOption("--user-assigned")]
    public string? UserAssigned { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}