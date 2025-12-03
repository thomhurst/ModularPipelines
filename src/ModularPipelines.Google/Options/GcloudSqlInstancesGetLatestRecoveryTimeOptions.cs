using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "instances", "get-latest-recovery-time")]
public record GcloudSqlInstancesGetLatestRecoveryTimeOptions(
[property: CliArgument] string Instance
) : GcloudOptions;