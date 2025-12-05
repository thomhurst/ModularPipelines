using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "certificates", "create")]
public record GcloudPrivatecaCertificatesCreateOptions(
[property: CliArgument] string Certificate,
[property: CliArgument] string IssuerLocation,
[property: CliArgument] string IssuerPool,
[property: CliOption("--cert-output-file")] string CertOutputFile,
[property: CliFlag("--validate-only")] bool ValidateOnly,
[property: CliOption("--csr")] string Csr,
[property: CliOption("--dns-san")] string[] DnsSan,
[property: CliOption("--email-san")] string[] EmailSan,
[property: CliOption("--ip-san")] string[] IpSan,
[property: CliOption("--subject")] string[] Subject,
[property: CliOption("--uri-san")] string[] UriSan,
[property: CliFlag("--generate-key")] bool GenerateKey,
[property: CliOption("--key-output-file")] string KeyOutputFile,
[property: CliOption("--kms-key-version")] string KmsKeyVersion,
[property: CliOption("--kms-key")] string KmsKey,
[property: CliOption("--kms-keyring")] string KmsKeyring,
[property: CliOption("--kms-location")] string KmsLocation,
[property: CliOption("--kms-project")] string KmsProject,
[property: CliOption("--use-preset-profile")] string UsePresetProfile,
[property: CliOption("--extended-key-usages")] string[] ExtendedKeyUsages,
[property: CliFlag("--is-ca-cert")] bool IsCaCert,
[property: CliOption("--key-usages")] string[] KeyUsages,
[property: CliOption("--max-chain-length")] string MaxChainLength,
[property: CliFlag("--unconstrained-chain-length")] bool UnconstrainedChainLength,
[property: CliFlag("--name-constraints-critical")] bool NameConstraintsCritical,
[property: CliOption("--name-excluded-dns")] string[] NameExcludedDns,
[property: CliOption("--name-excluded-email")] string[] NameExcludedEmail,
[property: CliOption("--name-excluded-ip")] string[] NameExcludedIp,
[property: CliOption("--name-excluded-uri")] string[] NameExcludedUri,
[property: CliOption("--name-permitted-dns")] string[] NamePermittedDns,
[property: CliOption("--name-permitted-email")] string[] NamePermittedEmail,
[property: CliOption("--name-permitted-ip")] string[] NamePermittedIp,
[property: CliOption("--name-permitted-uri")] string[] NamePermittedUri
) : GcloudOptions
{
    [CliOption("--ca")]
    public string? Ca { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--validity")]
    public string? Validity { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }

    [CliOption("--template-location")]
    public string? TemplateLocation { get; set; }
}