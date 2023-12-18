using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "troubleshoot", "command", "scvmm")]
public record AzArcapplianceTroubleshootCommandScvmmOptions : AzOptions
{
    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--credentials-dir")]
    public string? CredentialsDir { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }
}