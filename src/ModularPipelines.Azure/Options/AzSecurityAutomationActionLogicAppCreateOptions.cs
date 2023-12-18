using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "automation-action-logic-app", "create")]
public record AzSecurityAutomationActionLogicAppCreateOptions(
[property: CommandSwitch("--logic-app-resource-id")] string LogicAppResourceId,
[property: CommandSwitch("--uri")] string Uri
) : AzOptions
{
}

