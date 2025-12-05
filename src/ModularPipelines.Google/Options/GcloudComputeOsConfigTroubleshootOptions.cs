using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "troubleshoot")]
public record GcloudComputeOsConfigTroubleshootOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}