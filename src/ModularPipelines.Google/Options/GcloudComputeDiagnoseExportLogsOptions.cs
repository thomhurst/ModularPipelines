using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "diagnose", "export-logs")]
public record GcloudComputeDiagnoseExportLogsOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliFlag("--collect-process-traces")]
    public bool? CollectProcessTraces { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}