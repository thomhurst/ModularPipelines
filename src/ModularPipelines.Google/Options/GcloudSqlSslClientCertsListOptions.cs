using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "ssl", "client-certs", "list")]
public record GcloudSqlSslClientCertsListOptions(
[property: CliOption("--instance")] string Instance
) : GcloudOptions;