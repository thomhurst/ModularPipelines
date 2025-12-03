using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "subordinates", "create")]
public record GcloudPrivatecaSubordinatesCreateOptions(
[property: CliArgument] string CertificateAuthority,
[property: CliArgument] string Location,
[property: CliArgument] string Pool,
[property: CliFlag("--create-csr")] bool CreateCsr,
[property: CliOption("--csr-output-file")] string CsrOutputFile,
[property: CliOption("--issuer-pool")] string IssuerPool,
[property: CliOption("--issuer-location")] string IssuerLocation
) : GcloudOptions
{
    [CliFlag("--auto-enable")]
    public bool? AutoEnable { get; set; }

    [CliOption("--bucket")]
    public string? Bucket { get; set; }

    [CliOption("--dns-san")]
    public string[]? DnsSan { get; set; }

    [CliOption("--email-san")]
    public string[]? EmailSan { get; set; }

    [CliOption("--from-ca")]
    public string? FromCa { get; set; }

    [CliOption("--ip-san")]
    public string[]? IpSan { get; set; }

    [CliOption("--issuer-ca")]
    public string? IssuerCa { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--subject")]
    public string[]? Subject { get; set; }

    [CliOption("--uri-san")]
    public string[]? UriSan { get; set; }

    [CliOption("--validity")]
    public string? Validity { get; set; }

    [CliOption("--key-algorithm")]
    public string? KeyAlgorithm { get; set; }

    [CliOption("--kms-key-version")]
    public string? KmsKeyVersion { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }

    [CliOption("--use-preset-profile")]
    public string? UsePresetProfile { get; set; }

    [CliOption("--extended-key-usages")]
    public string[]? ExtendedKeyUsages { get; set; }

    [CliOption("--key-usages")]
    public string[]? KeyUsages { get; set; }

    [CliOption("--max-chain-length")]
    public string? MaxChainLength { get; set; }

    [CliFlag("--unconstrained-chain-length")]
    public bool? UnconstrainedChainLength { get; set; }

    [CliFlag("--name-constraints-critical")]
    public bool? NameConstraintsCritical { get; set; }

    [CliOption("--name-excluded-dns")]
    public string[]? NameExcludedDns { get; set; }

    [CliOption("--name-excluded-email")]
    public string[]? NameExcludedEmail { get; set; }

    [CliOption("--name-excluded-ip")]
    public string[]? NameExcludedIp { get; set; }

    [CliOption("--name-excluded-uri")]
    public string[]? NameExcludedUri { get; set; }

    [CliOption("--name-permitted-dns")]
    public string[]? NamePermittedDns { get; set; }

    [CliOption("--name-permitted-email")]
    public string[]? NamePermittedEmail { get; set; }

    [CliOption("--name-permitted-ip")]
    public string[]? NamePermittedIp { get; set; }

    [CliOption("--name-permitted-uri")]
    public string[]? NamePermittedUri { get; set; }
}