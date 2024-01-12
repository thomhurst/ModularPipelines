using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "reset-windows-password")]
public record GcloudComputeResetWindowsPasswordOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}