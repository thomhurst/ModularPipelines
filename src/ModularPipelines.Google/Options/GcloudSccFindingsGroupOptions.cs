using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "findings", "group")]
public record GcloudSccFindingsGroupOptions(
[property: CliArgument] string Parent
) : GcloudOptions
{
    [CliOption("--compare-duration")]
    public string? CompareDuration { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--group-by")]
    public string? GroupBy { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--read-time")]
    public string? ReadTime { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}