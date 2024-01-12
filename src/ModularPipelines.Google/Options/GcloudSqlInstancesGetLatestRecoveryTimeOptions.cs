using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "instances", "get-latest-recovery-time")]
public record GcloudSqlInstancesGetLatestRecoveryTimeOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions;