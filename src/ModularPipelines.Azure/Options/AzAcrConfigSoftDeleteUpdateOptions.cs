using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "config", "soft-delete", "update")]
public record AzAcrConfigSoftDeleteUpdateOptions(
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--days")]
    public int? Days { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}