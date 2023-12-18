using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "troubleshoot", "command", "hci")]
public record AzArcapplianceTroubleshootCommandHciOptions : AzOptions
{
    [CommandSwitch("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--credentials-dir")]
    public string? CredentialsDir { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }
}