using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "connected-env", "certificate", "upload")]
public record AzContainerappConnectedEnvCertificateUploadOptions(
[property: CliOption("--certificate-file")] string CertificateFile
) : AzOptions
{
    [CliOption("--certificate-name")]
    public string? CertificateName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--show-prompt")]
    public bool? ShowPrompt { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}