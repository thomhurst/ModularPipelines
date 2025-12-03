using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "ssl", "client-certs", "describe")]
public record GcloudSqlSslClientCertsDescribeOptions(
[property: CliArgument] string CommonName,
[property: CliOption("--instance")] string Instance
) : GcloudOptions;