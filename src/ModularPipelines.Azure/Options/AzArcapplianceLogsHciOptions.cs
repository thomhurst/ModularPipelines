using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcappliance", "logs", "hci")]
public record AzArcapplianceLogsHciOptions : AzOptions
{
    [CliOption("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CliOption("--credentials-dir")]
    public string? CredentialsDir { get; set; }

    [CliOption("--ip")]
    public string? Ip { get; set; }

    [CliOption("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CliOption("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }

    [CliOption("--out-dir")]
    public string? OutDir { get; set; }
}