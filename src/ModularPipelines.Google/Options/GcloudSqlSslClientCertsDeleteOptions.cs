using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "ssl", "client-certs", "delete")]
public record GcloudSqlSslClientCertsDeleteOptions(
[property: CliArgument] string CommonName,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}