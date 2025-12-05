using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CliCommand("source", "add")]
public record SourceAddOptions(
    [property: CliOption("--name")] string Name,
    [property: CliFlag("--arg")] bool Arg,
    [property: CliOption("--type")] string Type
) : WingetOptions
{
    [CliOption("--header")]
    public virtual string? Header { get; set; }

    [CliOption("--accept-source-agreements")]
    public virtual string? AcceptSourceAgreements { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }

    [CliFlag("--open-logs")]
    public virtual bool? OpenLogs { get; set; }

    [CliFlag("--verbose-logs")]
    public virtual bool? VerboseLogs { get; set; }

    [CliFlag("--disable-interactivity")]
    public virtual bool? DisableInteractivity { get; set; }
}