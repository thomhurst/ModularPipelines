using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("history")]
[ExcludeFromCodeCoverage]
public record HelmHistoryOptions : HelmOptions
{
    [CliOption("--max")]
    public int? Max { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }
}