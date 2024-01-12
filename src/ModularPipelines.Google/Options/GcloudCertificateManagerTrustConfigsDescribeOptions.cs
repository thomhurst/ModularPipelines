using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "trust-configs", "describe")]
public record GcloudCertificateManagerTrustConfigsDescribeOptions(
[property: PositionalArgument] string TrustConfig,
[property: PositionalArgument] string Location
) : GcloudOptions;