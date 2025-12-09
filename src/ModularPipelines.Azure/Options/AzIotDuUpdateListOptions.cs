using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "update", "list")]
public record AzIotDuUpdateListOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliFlag("--by-provider")]
    public bool? ByProvider { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--search")]
    public string? Search { get; set; }

    [CliOption("--un")]
    public string? Un { get; set; }

    [CliOption("--up")]
    public string? Up { get; set; }
}