using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "certificates", "describe")]
public record GcloudCertificateManagerCertificatesDescribeOptions(
[property: CliArgument] string Certificate,
[property: CliArgument] string Location
) : GcloudOptions;