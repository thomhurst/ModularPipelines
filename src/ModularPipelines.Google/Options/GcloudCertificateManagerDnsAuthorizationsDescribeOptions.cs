using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "dns-authorizations", "describe")]
public record GcloudCertificateManagerDnsAuthorizationsDescribeOptions(
[property: CliArgument] string DnsAuthorization,
[property: CliArgument] string Location
) : GcloudOptions;