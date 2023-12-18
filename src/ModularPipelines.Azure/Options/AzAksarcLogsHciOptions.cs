using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aksarc", "logs", "hci")]
public record AzAksarcLogsHciOptions(
[property: CommandSwitch("--credentials-dir")] string CredentialsDir
) : AzOptions
{
    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CommandSwitch("--out-dir")]
    public string? OutDir { get; set; }
}

