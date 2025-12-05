using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcappliance", "logs", "vmware")]
public record AzArcapplianceLogsVmwareOptions : AzOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--credentials-dir")]
    public string? CredentialsDir { get; set; }

    [CliOption("--ip")]
    public string? Ip { get; set; }

    [CliOption("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CliOption("--out-dir")]
    public string? OutDir { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}