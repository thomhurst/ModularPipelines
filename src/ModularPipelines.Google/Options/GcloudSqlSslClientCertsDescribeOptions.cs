using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "ssl", "client-certs", "describe")]
public record GcloudSqlSslClientCertsDescribeOptions(
[property: PositionalArgument] string CommonName,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;