using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "automation-scope", "create")]
public record AzSecurityAutomationScopeCreateOptions(
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--scope-path")] string ScopePath
) : AzOptions
{
}