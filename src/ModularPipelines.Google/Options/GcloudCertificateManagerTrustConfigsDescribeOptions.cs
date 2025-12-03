using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "trust-configs", "describe")]
public record GcloudCertificateManagerTrustConfigsDescribeOptions(
[property: CliArgument] string TrustConfig,
[property: CliArgument] string Location
) : GcloudOptions;