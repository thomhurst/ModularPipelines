using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "ssl-certificates", "delete")]
public record GcloudAppSslCertificatesDeleteOptions(
[property: CliArgument] string Id
) : GcloudOptions;