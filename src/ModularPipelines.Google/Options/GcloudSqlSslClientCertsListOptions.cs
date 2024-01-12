using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "ssl", "client-certs", "list")]
public record GcloudSqlSslClientCertsListOptions(
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;