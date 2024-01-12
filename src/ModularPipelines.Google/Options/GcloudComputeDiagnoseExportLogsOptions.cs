using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "diagnose", "export-logs")]
public record GcloudComputeDiagnoseExportLogsOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [BooleanCommandSwitch("--collect-process-traces")]
    public bool? CollectProcessTraces { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}