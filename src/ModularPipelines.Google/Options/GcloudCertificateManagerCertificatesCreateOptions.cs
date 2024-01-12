using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "certificates", "create")]
public record GcloudCertificateManagerCertificatesCreateOptions(
[property: PositionalArgument] string Certificate,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--certificate-file")] string CertificateFile,
[property: CommandSwitch("--private-key-file")] string PrivateKeyFile,
[property: CommandSwitch("--domains")] string[] Domains,
[property: CommandSwitch("--dns-authorizations")] string[] DnsAuthorizations,
[property: CommandSwitch("--issuance-config")] string IssuanceConfig
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}