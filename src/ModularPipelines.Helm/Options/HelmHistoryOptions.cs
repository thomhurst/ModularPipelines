using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("history")]
[ExcludeFromCodeCoverage]
public record HelmHistoryOptions : HelmOptions
{
    [CliOption("--max")]
    public virtual int? Max { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }
}