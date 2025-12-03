using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mysql", "flexible-server", "deploy", "run")]
public record AzMysqlFlexibleServerDeployRunOptions(
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--branch")] string Branch
) : AzOptions;