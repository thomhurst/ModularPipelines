using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "certificates", "create")]
public record GcloudPrivatecaCertificatesCreateOptions(
[property: PositionalArgument] string Certificate,
[property: PositionalArgument] string IssuerLocation,
[property: PositionalArgument] string IssuerPool,
[property: CommandSwitch("--cert-output-file")] string CertOutputFile,
[property: BooleanCommandSwitch("--validate-only")] bool ValidateOnly,
[property: CommandSwitch("--csr")] string Csr,
[property: CommandSwitch("--dns-san")] string[] DnsSan,
[property: CommandSwitch("--email-san")] string[] EmailSan,
[property: CommandSwitch("--ip-san")] string[] IpSan,
[property: CommandSwitch("--subject")] string[] Subject,
[property: CommandSwitch("--uri-san")] string[] UriSan,
[property: BooleanCommandSwitch("--generate-key")] bool GenerateKey,
[property: CommandSwitch("--key-output-file")] string KeyOutputFile,
[property: CommandSwitch("--kms-key-version")] string KmsKeyVersion,
[property: CommandSwitch("--kms-key")] string KmsKey,
[property: CommandSwitch("--kms-keyring")] string KmsKeyring,
[property: CommandSwitch("--kms-location")] string KmsLocation,
[property: CommandSwitch("--kms-project")] string KmsProject,
[property: CommandSwitch("--use-preset-profile")] string UsePresetProfile,
[property: CommandSwitch("--extended-key-usages")] string[] ExtendedKeyUsages,
[property: BooleanCommandSwitch("--is-ca-cert")] bool IsCaCert,
[property: CommandSwitch("--key-usages")] string[] KeyUsages,
[property: CommandSwitch("--max-chain-length")] string MaxChainLength,
[property: BooleanCommandSwitch("--unconstrained-chain-length")] bool UnconstrainedChainLength,
[property: BooleanCommandSwitch("--name-constraints-critical")] bool NameConstraintsCritical,
[property: CommandSwitch("--name-excluded-dns")] string[] NameExcludedDns,
[property: CommandSwitch("--name-excluded-email")] string[] NameExcludedEmail,
[property: CommandSwitch("--name-excluded-ip")] string[] NameExcludedIp,
[property: CommandSwitch("--name-excluded-uri")] string[] NameExcludedUri,
[property: CommandSwitch("--name-permitted-dns")] string[] NamePermittedDns,
[property: CommandSwitch("--name-permitted-email")] string[] NamePermittedEmail,
[property: CommandSwitch("--name-permitted-ip")] string[] NamePermittedIp,
[property: CommandSwitch("--name-permitted-uri")] string[] NamePermittedUri
) : GcloudOptions
{
    [CommandSwitch("--ca")]
    public string? Ca { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--validity")]
    public string? Validity { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [CommandSwitch("--template-location")]
    public string? TemplateLocation { get; set; }
}