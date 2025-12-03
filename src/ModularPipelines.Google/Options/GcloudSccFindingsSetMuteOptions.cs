using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "findings", "set-mute")]
public record GcloudSccFindingsSetMuteOptions(
[property: CliArgument] string Finding,
[property: CliOption("--mute")] string Mute
) : GcloudOptions
{
    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}