using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "ssl", "client-certs", "create")]
public record GcloudSqlSslClientCertsCreateOptions(
[property: CliArgument] string CommonName,
[property: CliArgument] string CertFile,
[property: CliOption("--instance")] string Instance
) : GcloudOptions;