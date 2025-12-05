using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "certificates", "create")]
public record GcloudCertificateManagerCertificatesCreateOptions(
[property: CliArgument] string Certificate,
[property: CliArgument] string Location,
[property: CliOption("--certificate-file")] string CertificateFile,
[property: CliOption("--private-key-file")] string PrivateKeyFile,
[property: CliOption("--domains")] string[] Domains,
[property: CliOption("--dns-authorizations")] string[] DnsAuthorizations,
[property: CliOption("--issuance-config")] string IssuanceConfig
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }
}