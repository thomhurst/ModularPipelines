using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "automation-action-logic-app", "create")]
public record AzSecurityAutomationActionLogicAppCreateOptions(
[property: CommandSwitch("--logic-app-resource-id")] string LogicAppResourceId,
[property: CommandSwitch("--uri")] string Uri
) : AzOptions;