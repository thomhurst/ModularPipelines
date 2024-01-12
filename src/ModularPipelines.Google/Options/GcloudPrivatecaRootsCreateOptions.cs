using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "roots", "create")]
public record GcloudPrivatecaRootsCreateOptions(
[property: PositionalArgument] string CertificateAuthority,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Pool
) : GcloudOptions
{
    [BooleanCommandSwitch("--auto-enable")]
    public bool? AutoEnable { get; set; }

    [CommandSwitch("--bucket")]
    public string? Bucket { get; set; }

    [CommandSwitch("--dns-san")]
    public string[]? DnsSan { get; set; }

    [CommandSwitch("--email-san")]
    public string[]? EmailSan { get; set; }

    [CommandSwitch("--from-ca")]
    public string? FromCa { get; set; }

    [CommandSwitch("--ip-san")]
    public string[]? IpSan { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--subject")]
    public string[]? Subject { get; set; }

    [CommandSwitch("--uri-san")]
    public string[]? UriSan { get; set; }

    [CommandSwitch("--validity")]
    public string? Validity { get; set; }

    [CommandSwitch("--key-algorithm")]
    public string? KeyAlgorithm { get; set; }

    [CommandSwitch("--kms-key-version")]
    public string? KmsKeyVersion { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }

    [CommandSwitch("--use-preset-profile")]
    public string? UsePresetProfile { get; set; }

    [CommandSwitch("--extended-key-usages")]
    public string[]? ExtendedKeyUsages { get; set; }

    [CommandSwitch("--key-usages")]
    public string[]? KeyUsages { get; set; }

    [CommandSwitch("--max-chain-length")]
    public string? MaxChainLength { get; set; }

    [BooleanCommandSwitch("--unconstrained-chain-length")]
    public bool? UnconstrainedChainLength { get; set; }

    [BooleanCommandSwitch("--name-constraints-critical")]
    public bool? NameConstraintsCritical { get; set; }

    [CommandSwitch("--name-excluded-dns")]
    public string[]? NameExcludedDns { get; set; }

    [CommandSwitch("--name-excluded-email")]
    public string[]? NameExcludedEmail { get; set; }

    [CommandSwitch("--name-excluded-ip")]
    public string[]? NameExcludedIp { get; set; }

    [CommandSwitch("--name-excluded-uri")]
    public string[]? NameExcludedUri { get; set; }

    [CommandSwitch("--name-permitted-dns")]
    public string[]? NamePermittedDns { get; set; }

    [CommandSwitch("--name-permitted-email")]
    public string[]? NamePermittedEmail { get; set; }

    [CommandSwitch("--name-permitted-ip")]
    public string[]? NamePermittedIp { get; set; }

    [CommandSwitch("--name-permitted-uri")]
    public string[]? NamePermittedUri { get; set; }
}