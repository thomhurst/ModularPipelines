using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "flexible-server", "deploy", "run")]
public record AzPostgresFlexibleServerDeployRunOptions(
[property: CommandSwitch("--action-name")] string ActionName,
[property: CommandSwitch("--branch")] string Branch
) : AzOptions
{
}