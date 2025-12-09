using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "secret", "format")]
public record AzVmSecretFormatOptions(
[property: CliOption("--secrets")] string Secrets
) : AzOptions
{
    [CliOption("--certificate-store")]
    public string? CertificateStore { get; set; }

    [CliOption("--keyvault")]
    public string? Keyvault { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}