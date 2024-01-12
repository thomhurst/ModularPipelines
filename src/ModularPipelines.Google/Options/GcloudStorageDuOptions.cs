using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "du")]
public record GcloudStorageDuOptions(
[property: PositionalArgument] string Url
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--all-versions")]
    public bool? AllVersions { get; set; }

    [CommandSwitch("--exclude-name-pattern")]
    public string? ExcludeNamePattern { get; set; }

    [CommandSwitch("--exclude-name-pattern-file")]
    public string? ExcludeNamePatternFile { get; set; }

    [BooleanCommandSwitch("--readable-sizes")]
    public bool? ReadableSizes { get; set; }

    [BooleanCommandSwitch("--summarize")]
    public bool? Summarize { get; set; }

    [BooleanCommandSwitch("--total")]
    public bool? Total { get; set; }

    [BooleanCommandSwitch("--zero-terminator")]
    public bool? ZeroTerminator { get; set; }
}