using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "config", "retention", "update")]
public record AzAcrConfigRetentionUpdateOptions(
[property: CliOption("--registry")] string Registry,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--days")]
    public int? Days { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }
}