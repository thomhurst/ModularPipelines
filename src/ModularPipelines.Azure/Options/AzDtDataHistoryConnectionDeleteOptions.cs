using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "data-history", "connection", "delete")]
public record AzDtDataHistoryConnectionDeleteOptions(
[property: CliOption("--cn")] string Cn,
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliFlag("--clean")]
    public bool? Clean { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}