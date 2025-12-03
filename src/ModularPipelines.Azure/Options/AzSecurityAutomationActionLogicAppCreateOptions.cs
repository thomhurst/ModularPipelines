using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "automation-action-logic-app", "create")]
public record AzSecurityAutomationActionLogicAppCreateOptions(
[property: CliOption("--logic-app-resource-id")] string LogicAppResourceId,
[property: CliOption("--uri")] string Uri
) : AzOptions;