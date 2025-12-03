using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "dns-authorizations", "create")]
public record GcloudCertificateManagerDnsAuthorizationsCreateOptions(
[property: CliArgument] string DnsAuthorization,
[property: CliArgument] string Location,
[property: CliOption("--domain")] string Domain
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}