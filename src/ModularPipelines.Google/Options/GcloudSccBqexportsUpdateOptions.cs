using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "bqexports", "update")]
public record GcloudSccBqexportsUpdateOptions(
[property: CliArgument] string BigQueryExport
) : GcloudOptions
{
    [CliOption("--dataset")]
    public string? Dataset { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--update-mask")]
    public string? UpdateMask { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}