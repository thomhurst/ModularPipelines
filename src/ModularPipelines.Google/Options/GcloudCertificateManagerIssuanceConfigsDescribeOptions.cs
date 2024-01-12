using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "issuance-configs", "describe")]
public record GcloudCertificateManagerIssuanceConfigsDescribeOptions(
[property: PositionalArgument] string CertificateIssuanceConfig,
[property: PositionalArgument] string Location
) : GcloudOptions;