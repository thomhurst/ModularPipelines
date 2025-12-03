using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "findings", "list-marks")]
public record GcloudSccFindingsListMarksOptions(
[property: CliArgument] string Finding,
[property: CliArgument] string Organization,
[property: CliArgument] string Source
) : GcloudOptions
{
    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--read-time")]
    public string? ReadTime { get; set; }
}