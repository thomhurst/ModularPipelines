using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dls", "fs", "upload")]
public record AzDlsFsUploadOptions(
[property: CliOption("--destination-path")] string DestinationPath,
[property: CliOption("--source-path")] string SourcePath
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--block-size")]
    public string? BlockSize { get; set; }

    [CliOption("--buffer-size")]
    public string? BufferSize { get; set; }

    [CliOption("--chunk-size")]
    public string? ChunkSize { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--overwrite")]
    public bool? Overwrite { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--thread-count")]
    public int? ThreadCount { get; set; }
}