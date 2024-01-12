using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "dns-authorizations", "describe")]
public record GcloudCertificateManagerDnsAuthorizationsDescribeOptions(
[property: PositionalArgument] string DnsAuthorization,
[property: PositionalArgument] string Location
) : GcloudOptions;