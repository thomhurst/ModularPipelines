using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dls", "fs", "download")]
public record AzDlsFsDownloadOptions(
[property: CommandSwitch("--destination-path")] string DestinationPath,
[property: CommandSwitch("--source-path")] string SourcePath
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--block-size")]
    public string? BlockSize { get; set; }

    [CommandSwitch("--buffer-size")]
    public string? BufferSize { get; set; }

    [CommandSwitch("--chunk-size")]
    public string? ChunkSize { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--thread-count")]
    public int? ThreadCount { get; set; }
}