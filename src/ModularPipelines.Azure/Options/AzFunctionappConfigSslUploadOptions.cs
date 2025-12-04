using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "config", "ssl", "upload")]
public record AzFunctionappConfigSslUploadOptions(
[property: CliOption("--certificate-file")] string CertificateFile,
[property: CliOption("--certificate-password")] string CertificatePassword
) : AzOptions
{
    [CliOption("--certificate-name")]
    public string? CertificateName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}