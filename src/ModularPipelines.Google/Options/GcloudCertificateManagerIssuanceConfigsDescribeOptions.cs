using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "issuance-configs", "describe")]
public record GcloudCertificateManagerIssuanceConfigsDescribeOptions(
[property: CliArgument] string CertificateIssuanceConfig,
[property: CliArgument] string Location
) : GcloudOptions;