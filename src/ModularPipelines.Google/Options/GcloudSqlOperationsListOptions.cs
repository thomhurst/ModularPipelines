using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "operations", "list")]
public record GcloudSqlOperationsListOptions(
[property: CliOption("--instance")] string Instance
) : GcloudOptions;