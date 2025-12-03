using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "config", "ssl", "bind")]
public record AzWebappConfigSslBindOptions(
[property: CliOption("--certificate-thumbprint")] string CertificateThumbprint,
[property: CliOption("--ssl-type")] string SslType
) : AzOptions
{
    [CliOption("--hostname")]
    public string? Hostname { get; set; }

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