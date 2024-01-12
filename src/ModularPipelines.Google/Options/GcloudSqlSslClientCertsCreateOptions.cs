using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "ssl", "client-certs", "create")]
public record GcloudSqlSslClientCertsCreateOptions(
[property: PositionalArgument] string CommonName,
[property: PositionalArgument] string CertFile,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;