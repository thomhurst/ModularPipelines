using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dls", "fs", "preview")]
public record AzDlsFsPreviewOptions(
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--length")]
    public string? Length { get; set; }

    [CliOption("--offset")]
    public string? Offset { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}