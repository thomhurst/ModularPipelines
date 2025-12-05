using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "flexible-server", "deploy", "run")]
public record AzMysqlFlexibleServerDeployRunOptions(
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--branch")] string Branch
) : AzOptions;