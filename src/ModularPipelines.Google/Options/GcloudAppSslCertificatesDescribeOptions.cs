using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "ssl-certificates", "describe")]
public record GcloudAppSslCertificatesDescribeOptions(
[property: CliArgument] string Id
) : GcloudOptions;