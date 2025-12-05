using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "findings", "list")]
public record GcloudSccFindingsListOptions(
[property: CliArgument] string Parent
) : GcloudOptions
{
    [CliOption("--compare-duration")]
    public string? CompareDuration { get; set; }

    [CliOption("--field-mask")]
    public string? FieldMask { get; set; }

    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--read-time")]
    public string? ReadTime { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}