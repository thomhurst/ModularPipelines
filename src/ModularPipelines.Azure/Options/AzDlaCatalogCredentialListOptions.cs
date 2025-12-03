using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dla", "catalog", "credential", "list")]
public record AzDlaCatalogCredentialListOptions(
[property: CliOption("--database-name")] string DatabaseName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--count")]
    public int? Count { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--select")]
    public string? Select { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}