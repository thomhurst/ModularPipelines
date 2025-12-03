using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "certificates", "update")]
public record GcloudCertificateManagerCertificatesUpdateOptions(
[property: CliArgument] string Certificate,
[property: CliArgument] string Location
) : GcloudOptions;