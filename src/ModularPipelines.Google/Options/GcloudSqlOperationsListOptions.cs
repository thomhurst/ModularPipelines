using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "operations", "list")]
public record GcloudSqlOperationsListOptions(
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;