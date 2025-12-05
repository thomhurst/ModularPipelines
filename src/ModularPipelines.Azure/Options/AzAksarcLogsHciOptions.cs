using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aksarc", "logs", "hci")]
public record AzAksarcLogsHciOptions(
[property: CliOption("--credentials-dir")] string CredentialsDir
) : AzOptions
{
    [CliOption("--ip")]
    public string? Ip { get; set; }

    [CliOption("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CliOption("--out-dir")]
    public string? OutDir { get; set; }
}