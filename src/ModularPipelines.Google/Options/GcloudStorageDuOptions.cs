using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "du")]
public record GcloudStorageDuOptions(
[property: CliArgument] string Url
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--all-versions")]
    public bool? AllVersions { get; set; }

    [CliOption("--exclude-name-pattern")]
    public string? ExcludeNamePattern { get; set; }

    [CliOption("--exclude-name-pattern-file")]
    public string? ExcludeNamePatternFile { get; set; }

    [CliFlag("--readable-sizes")]
    public bool? ReadableSizes { get; set; }

    [CliFlag("--summarize")]
    public bool? Summarize { get; set; }

    [CliFlag("--total")]
    public bool? Total { get; set; }

    [CliFlag("--zero-terminator")]
    public bool? ZeroTerminator { get; set; }
}