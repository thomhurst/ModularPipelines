using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "ssl-certificates", "list")]
public record GcloudAppSslCertificatesListOptions : GcloudOptions;