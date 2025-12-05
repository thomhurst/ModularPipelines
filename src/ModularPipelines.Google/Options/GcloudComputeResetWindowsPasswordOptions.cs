using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "reset-windows-password")]
public record GcloudComputeResetWindowsPasswordOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--user")]
    public string? User { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}