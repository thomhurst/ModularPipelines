using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "troubleshoot", "command", "vmware")]
public record AzArcapplianceTroubleshootCommandVmwareOptions : AzOptions
{
    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--credentials-dir")]
    public string? CredentialsDir { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}