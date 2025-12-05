using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "send-diagnostic-interrupt")]
public record GcloudComputeInstancesSendDiagnosticInterruptOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}