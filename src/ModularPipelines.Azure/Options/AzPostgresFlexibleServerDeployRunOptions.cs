using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "flexible-server", "deploy", "run")]
public record AzPostgresFlexibleServerDeployRunOptions(
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--branch")] string Branch
) : AzOptions;