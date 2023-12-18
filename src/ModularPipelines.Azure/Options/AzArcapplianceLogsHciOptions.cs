using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "logs", "hci")]
public record AzArcapplianceLogsHciOptions : AzOptions
{
    [CommandSwitch("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CommandSwitch("--credentials-dir")]
    public string? CredentialsDir { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CommandSwitch("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }

    [CommandSwitch("--out-dir")]
    public string? OutDir { get; set; }
}